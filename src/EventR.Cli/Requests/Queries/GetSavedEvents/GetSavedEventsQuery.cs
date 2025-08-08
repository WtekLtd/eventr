using EventR.Cli.DTOs;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetSavedEvents;

public record GetSavedEventsQuery : IQuery<ICollection<SavedEventDto>>
{
    
}
