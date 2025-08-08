using EventR.Cli.DTOs;
using EventR.Cli.Models;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetSavedEventLocations;

public class GetSavedEventLocationsHandler(
    IRepository<SavedEventLocation> savedEventLocationRepository
) : IQueryHandler<GetSavedEventLocationsQuery, ICollection<SavedEventLocationDto>>
{
    public ICollection<SavedEventLocationDto> Handle(GetSavedEventLocationsQuery query)
    {
        return [
            ..savedEventLocationRepository.GetAll()
                .Select(SavedEventLocationDto.FromModel)
        ];
    }
}