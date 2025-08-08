using EventR.Cli.DTOs;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetEndpoint;

public class GetEndpointQueryHandler(
    IRepository<Models.Endpoint> endpointRepository
) : IQueryHandler<GetEndpointQuery, EndpointDto?>
{
    public EndpointDto? Handle(GetEndpointQuery query)
    {
        var endpoint = endpointRepository.GetByIdentifier(query.EndpointIdentifier);
        return endpoint == null ? null : EndpointDto.FromModel(endpoint);
    }
}