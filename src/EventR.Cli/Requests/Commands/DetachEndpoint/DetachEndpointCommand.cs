using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Commands.DetachEndpoint;

public record DetachEndpointCommand : ICommand
{
    public required string EndpointIdentifier { get; init; }
}
