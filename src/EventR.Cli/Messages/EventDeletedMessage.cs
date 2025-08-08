namespace EventR.Cli.Messages;

public record EventDeletedMessage
{
    public required string EventIdentifier { get; init; }
}