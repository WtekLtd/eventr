using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Commands.DeleteEvent;

public record DeleteEventCommand : ICommand
{
    public required string EventIdentifier { get; init; }
}
