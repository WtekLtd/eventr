using EventR.Cli.DTOs;

namespace EventR.Cli.Messages;

public record EventSavedMessage
{
    public required SavedEventDto SavedEvent { get; init; }
}
