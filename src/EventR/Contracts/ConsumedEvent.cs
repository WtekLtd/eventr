namespace EventR.Contracts;

public record ConsumedEvent : Event
{
    public required object Metadata { get; init; }
}