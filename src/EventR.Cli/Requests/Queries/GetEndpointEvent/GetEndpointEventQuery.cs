using EventR.Cli.DTOs;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetEndpointEvent;

public record GetEndpointEventQuery : IQuery<EndpointEventDto?>
{
    public required string EventIdentifier { get; init; }
}
