using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Commands.PingEndpoint;

public record PingEndpointCommand : ICommand
{
    public required string EndpointIdentifier { get; init; }
}
