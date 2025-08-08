using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Commands.SaveEventAs;

public record SaveEventAsCommand : ICommand
{
    public required string LocationIdentifier { get; init; }

    public required string Name { get; init; }

    public required string Data { get; init; }
}
