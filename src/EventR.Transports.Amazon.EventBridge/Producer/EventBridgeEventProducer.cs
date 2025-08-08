using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using EventR.Contracts;
using Microsoft.Extensions.Options;

namespace EventR.Transports.Amazon.EventBridge.Producer;

public class EventBridgeEventProducer(
    IAmazonEventBridge amazonEventBridge,
    IOptions<EventBridgeEventProducerOptions> options
) : IEventProducer
{
    public async Task ProduceEventsAsync(IEnumerable<ProducedEvent> events, CancellationToken cancellationToken)
    {
        foreach (var eventBatch in events.Chunk(10))
        {
            await amazonEventBridge.PutEventsAsync(
                new PutEventsRequest
                {
                    EndpointId = options.Value.EndpointId,
                    Entries = [.. eventBatch.Select(e => new PutEventsRequestEntry
                    {
                        Detail = e.Data,
                        DetailType = e.Type,
                        EventBusName = options.Value.EventBusName
                    })]
                },
                cancellationToken
            );
        }
    }
}