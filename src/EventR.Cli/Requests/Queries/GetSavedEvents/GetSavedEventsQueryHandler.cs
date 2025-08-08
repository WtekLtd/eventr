using EventR.Cli.DTOs;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetSavedEvents;

public class GetSavedEventsQueryHandler(
    ISavedEventRepository savedEventRepository
) : IQueryHandler<GetSavedEventsQuery, ICollection<SavedEventDto>>
{
    public ICollection<SavedEventDto> Handle(GetSavedEventsQuery query)
    {
        return [
            ..savedEventRepository.GetAll()
                .Select(SavedEventDto.FromModel)
        ];
    }
}