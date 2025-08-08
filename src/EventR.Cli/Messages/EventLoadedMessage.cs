using EventR.Cli.DTOs;

namespace EventR.Cli.Messages;

public record EventLoadedMessage
{
    public required SavedEventDto SavedEvent { get; init; }
}