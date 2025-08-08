using EventR.Cli.DTOs;
using EventR.Cli.Models;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetEndpointEventsForEndpoint;

public class GetEndpointEventsForEndpointQueryHandler(
    IRepository<EndpointEvent> endpointEventRepository
) : IQueryHandler<GetEndpointEventsForEndpointQuery, ICollection<EndpointEventDto>>
{
    public ICollection<EndpointEventDto> Handle(GetEndpointEventsForEndpointQuery query)
    {
        var models = endpointEventRepository.GetAll();
        return [
            ..endpointEventRepository.GetAll()
                .Where(ep => ep.Endpoint?.Identifier == query.EndpointIdentifier)
                .Select(EndpointEventDto.FromModel)
        ];
    }
}