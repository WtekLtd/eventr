using EventR.Contracts;
using EventR.Implementations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventR;

public class EventRBackgroundService(
    IEventSource eventSource,
    IEventProducer eventProducer,
    IEventProcessor eventProcessor,
    ILogger<EventRBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await eventSource.SubscribeAsync(stoppingToken);

        int[] backoutSeconds = [1, 1];

        while (!stoppingToken.IsCancellationRequested)
        {
            ConsumedEvent? consumedEvent = null;
            try
            {
                consumedEvent = await eventSource.GetEventAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred fetching event.");
                await Task.Delay(backoutSeconds[1] * 1000, stoppingToken);
                backoutSeconds = [
                    backoutSeconds[1],
                    Math.Min(backoutSeconds[0] + backoutSeconds[1], 13)
                ];
            }

            if (consumedEvent == null)
            {
                continue;
            }

            backoutSeconds = [1, 1];

            try
            {
                var context = new EventProcessorContext(consumedEvent);
                await eventProcessor.ProcessEventAsync(context, stoppingToken);
                if (context.OutgoingEvents.Count > 0)
                {
                    await eventProducer.ProduceEventsAsync(context.OutgoingEvents, stoppingToken);
                }
                await eventSource.FinalizeEventAsync(consumedEvent, stoppingToken);
            }
            catch (Exception ex)
            {
                await eventSource.FailEventAsync(consumedEvent, ex.Message, stoppingToken);
            }
        }

        await eventSource.UnsubscribeAsync(CancellationToken.None);
    }
}