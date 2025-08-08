using EventR.Cli.Constants;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Commands.PublishEndpointEvent;

public record PublishEndpointEventCommand : ICommand
{
    public required string EndpointIdentifier { get; init; }

    public required string Data { get; init; }

    public required string? Message { get; init; }

    public required EventStatus Status { get; init; }
}
