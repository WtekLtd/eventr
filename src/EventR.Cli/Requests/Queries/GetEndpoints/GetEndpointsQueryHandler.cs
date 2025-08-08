using EventR.Cli.DTOs;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetEndpoints;

public class GetEndpointsQueryHandler(
    IRepository<Models.Endpoint> endpointRepository
) : IQueryHandler<GetEndpointsQuery, ICollection<EndpointDto>>
{
    public ICollection<EndpointDto> Handle(GetEndpointsQuery query)
    {
        return [
            ..endpointRepository.GetAll()
                .Select(EndpointDto.FromModel)
        ];
    }
}