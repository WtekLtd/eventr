using EventR.Contracts;
using EventR.Transports.Development.Configuration;
using Microsoft.Extensions.Options;

namespace EventR.Transports.Development;

public class DebugEventProducer(
    IDebugServiceClient debugServiceClient,
    IOptions<DevelopmentTransportOptions> options) : IEventProducer
{
    public async Task ProduceEventsAsync(IEnumerable<ProducedEvent> events, CancellationToken cancellationToken)
    {
        foreach (var evnt in events)
        {
            await debugServiceClient.PublishEventAsync(
                endpointIdentifier: options.Value.Identifier,
                request: new()
                {
                    Data = evnt.Data
                },
                cancellationToken
            );
        }
    }
}