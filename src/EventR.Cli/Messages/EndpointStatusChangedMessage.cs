using EventR.Cli.Constants;

namespace EventR.Cli.Messages;

public record EndpointStatusChangedMessage
{
    public required string EndpointIdentifier { get; init; }

    public required EndpointStatus Status { get; init; }
}