using EventR.Contracts;
using EventR.Cli.Interface.Requests;
using EventR.Transports.Development.Configuration;
using Microsoft.Extensions.Options;

namespace EventR.Transports.Development;

public class DebugEventSource(
    IDebugServiceClient debugServiceClient,
    IOptions<DevelopmentTransportOptions> options) : IEventSource
{
    public async Task FailEventAsync(ConsumedEvent evnt, string message, CancellationToken cancellationToken)
    {
        if (evnt.Metadata is not DebugEventMetaData metaData)
        {
            return;
        }

        await debugServiceClient.UpdateEventStatusAsync(
            eventIdentifier: metaData.EventIdentifier,
            request: new()
            {
                IsSuccess = false,
                Message = message
            },
            cancellationToken
        );
    }

    public async Task FinalizeEventAsync(ConsumedEvent evnt, CancellationToken cancellationToken)
    {
        if (evnt.Metadata is not DebugEventMetaData metaData)
        {
            return;
        }

        await debugServiceClient.UpdateEventStatusAsync(
            eventIdentifier: metaData.EventIdentifier,
            request: new()
            {
                IsSuccess = true,
            },
            cancellationToken
        );
    }

    public async Task<ConsumedEvent?> GetEventAsync(CancellationToken cancellationToken)
    {
        var response = await debugServiceClient.GetEventAsync(
            endpointIdentifier: options.Value.Identifier,
            cancellationToken
        );

        if (response == null)
        {
            return null;
        }

        return new()
        {
            Data = response.Data,
            Metadata = new DebugEventMetaData
            {
                EventIdentifier = response.Identifier
            }
        };
    }

    public async Task SubscribeAsync(CancellationToken cancellationToken)
    {
        var optionsValue = options.Value;
        var endpointRegistered = false;
        var firstAttempt = true;

        while (!endpointRegistered && !cancellationToken.IsCancellationRequested)
        {
            var savedEventPath = Path.Join(Environment.CurrentDirectory, optionsValue.SavedEventPath);

            endpointRegistered = await debugServiceClient.RegisterEndpointAsync(
                new()
                {
                    Identifier = optionsValue.Identifier,
                    Name = optionsValue.Name,
                    SavedEventPath = savedEventPath,
                    Columns =
                    [
                        ..optionsValue.Columns
                            .Select(c => new EndpointDataColumnRequest
                            {
                                Title = c.Title,
                                Pointer = c.Pointer
                            })
                    ]
                },
                cancellationToken
            );

            if (!endpointRegistered)
            {
                if (firstAttempt)
                {
                    Console.WriteLine("Waiting for debugger to become available...");
                }

                firstAttempt = false;
                await Task.Delay(1_000, cancellationToken);
            }
        }
    }

    public async Task UnsubscribeAsync(CancellationToken cancellationToken)
    {
        await debugServiceClient.DetachEndpointAsync(options.Value.Identifier, cancellationToken);
    }
}
