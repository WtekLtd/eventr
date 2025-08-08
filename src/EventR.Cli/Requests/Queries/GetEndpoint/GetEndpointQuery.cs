using EventR.Cli.DTOs;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetEndpoint;

public record GetEndpointQuery : IQuery<EndpointDto?>
{
    public required string EndpointIdentifier { get; init; }
}
