using EventR.Cli.Services.RequestDispatch;
using EventR.Cli.Interface.Responses;

namespace EventR.Cli.Requests.Queries.DequeueEndpointEvent;

public record DequeueEndpointEventQuery : IAsyncQuery<GetEventResponse?>
{
    public required string EndpointIdentifier { get; init; }
}
