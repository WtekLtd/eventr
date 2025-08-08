namespace EventR.Contracts;

public record ProducedEvent : Event
{
    public required string Type { get; init; }
}
