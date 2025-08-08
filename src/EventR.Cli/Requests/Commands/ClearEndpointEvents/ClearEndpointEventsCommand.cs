using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Commands.ClearEndpointEvents;

public class ClearEndpointEventsCommand : ICommand
{
    public required string? EndpointIdentifier { get; init; }
}
