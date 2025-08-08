using EventR.Cli.DTOs;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetSavedEvent;

public class GetSavedEventQueryHandler(
    ISavedEventRepository savedEventRepository
) : IQueryHandler<GetSavedEventQuery, SavedEventDto?>
{
    public SavedEventDto? Handle(GetSavedEventQuery query)
    {
        var model = savedEventRepository.GetByIdentifier(query.SavedEventIdentifier);
        return model == null ? null : SavedEventDto.FromModel(model);
    }
}