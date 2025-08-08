using EventR.Cli.DTOs;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetSavedEventLocations;

public record GetSavedEventLocationsQuery : IQuery<ICollection<SavedEventLocationDto>>
{
    
}
