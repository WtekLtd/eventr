namespace EventR.Cli.Messages;

public record EventsClearedMessage
{
    public required string? EndpointIdentifier { get; init; }
}