using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Commands.UpdateEndpointEventStatus;

public record UpdateEndpointEventStatusCommand : ICommand
{
    public required string EventIdentifier { get; init; }

    public required bool IsSuccess { get; init; }

    public required string? Message { get; init; }
}
