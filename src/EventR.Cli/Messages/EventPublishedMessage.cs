using EventR.Cli.DTOs;

namespace EventR.Cli.Messages;

public record EventPublishedMessage
{
    public required EndpointEventDto EndpointEvent { get; init; }
}
