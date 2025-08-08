using EventR.Cli.DTOs;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetEndpointEvents;

public record GetEndpointEventsQuery : IQuery<ICollection<EndpointEventDto>>
{
}
