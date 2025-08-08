using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Commands.SaveEvent;

public record SaveEventCommand : ICommand
{
    public required string EventIdentifier { get; init; }

    public required string Data { get; init; }
}
