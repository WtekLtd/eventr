using EventR.Cli.DTOs;
using EventR.Cli.Models;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetEndpointEvent;

public class GetEndpointEventQueryHandler(
    IRepository<EndpointEvent> endpointEventRepository
) : IQueryHandler<GetEndpointEventQuery, EndpointEventDto?>
{
    public EndpointEventDto? Handle(GetEndpointEventQuery query)
    {
        var model = endpointEventRepository.GetByIdentifier(query.EventIdentifier);
        return model == null ? null : EndpointEventDto.FromModel(model);
    }
}