using EventR.Cli.DTOs;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Queries.GetSavedEvent;

public record GetSavedEventQuery : IQuery<SavedEventDto?>
{
    public required string SavedEventIdentifier { get; init; }
}
