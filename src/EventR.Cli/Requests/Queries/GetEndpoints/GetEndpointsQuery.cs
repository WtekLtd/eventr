using EventR.Cli.DTOs;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetEndpoints;

public record GetEndpointsQuery : IQuery<ICollection<EndpointDto>>
{
    
}
