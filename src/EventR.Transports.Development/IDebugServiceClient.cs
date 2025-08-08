using EventR.Cli.Interface.Requests;
using EventR.Cli.Interface.Responses;

namespace EventR.Transports.Development;

public interface IDebugServiceClient
{
    Task<bool> RegisterEndpointAsync(RegisterEndpointRequest request, CancellationToken cancellationToken);

    public Task DetachEndpointAsync(string endpointIdentifier, CancellationToken cancellationToken);

    Task PublishEventAsync(string endpointIdentifier, PublishEventRequest request, CancellationToken cancellationToken);

    Task PublishLogAsync(string endpointIdentifier, PublishLogRequest request, CancellationToken cancellationToken);

    Task UpdateEventStatusAsync(string eventIdentifier, UpdateEventStatusRequest request, CancellationToken cancellationToken);

    Task<GetEventResponse?> GetEventAsync(string endpointIdentifier, CancellationToken cancellationToken);
}
