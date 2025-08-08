using System.Text.Json;
using EventR.Contracts;
using Microsoft.Extensions.Logging;

namespace EventR.Demo.Service;

public class DemoEventProcessor(ILogger<DemoEventProcessor> logger) : IEventProcessor
{
    public async Task ProcessEventAsync(IEventProcessorContext context, CancellationToken cancellationToken)
    {
        logger.LogInformation("An event has been received.");

        await Task.Delay(5000, cancellationToken);

        var eventDocument = JsonDocument.Parse(context.IncomingEvent.Data);
        var detailType = eventDocument.RootElement.GetProperty("detail-type").GetString();
        if (detailType == "FailureMessage")
        {
            var message = eventDocument.RootElement
                .GetProperty("detail")
                .GetProperty("message")
                .GetString() ?? "UNKNOWN";
            logger.LogError("Something bad happened : {Message}", message);
            throw new InvalidDataException("FailureMessage");
        }

        logger.LogInformation("Event succesfully processed.");
        context.OutgoingEvents.Add(new()
        {
            Type = "SuccessMessageResponse",
            Data = """
            {
                "detail_type": "SuccessMessageResponse",
                "detail": {}
            }
            """
        });
    }
}