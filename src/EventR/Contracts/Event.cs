namespace EventR.Contracts;

public abstract record Event
{
    public required string Data { get; init; }
}
