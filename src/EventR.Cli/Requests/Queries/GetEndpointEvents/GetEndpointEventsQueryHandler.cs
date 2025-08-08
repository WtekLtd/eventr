using EventR.Cli.DTOs;
using EventR.Cli.Models;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetEndpointEvents;

public class GetEndpointEventsQueryHandler(
    IRepository<EndpointEvent> endpointEventRepository
) : IQueryHandler<GetEndpointEventsQuery, ICollection<EndpointEventDto>>
{
    public ICollection<EndpointEventDto> Handle(GetEndpointEventsQuery query)
    {
        var models = endpointEventRepository.GetAll();
        return [
            ..endpointEventRepository.GetAll()
                .Select(EndpointEventDto.FromModel)
        ];
    }
}