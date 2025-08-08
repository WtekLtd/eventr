using EventR.Cli.DTOs;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetEndpointEventsForEndpoint;

public record GetEndpointEventsForEndpointQuery : IQuery<ICollection<EndpointEventDto>>
{
    public required string EndpointIdentifier { get; init; }
}
