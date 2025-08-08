using EventR.Cli.DTOs;

namespace EventR.Cli.Messages;

public record EndpointRegisteredMessage
{
    public required EndpointDto Endpoint { get; init; }
}