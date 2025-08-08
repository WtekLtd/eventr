using EventR.Cli.Constants;

namespace EventR.Cli.Messages;

public record EventStatusChangedMessage
{
    public required string EventIdentifier { get; init; }

    public required EventStatus Status { get; init; }

    public required string? Message { get; init; }
}
