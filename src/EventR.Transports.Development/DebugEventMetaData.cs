namespace EventR.Transports.Development;

public record DebugEventMetaData
{
    public required string EventIdentifier { get; init; }
}