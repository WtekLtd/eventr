namespace EventR.Cli.Models;

public record SavedEvent : BaseModel
{
    public required string Name { get; init; }

    public required string Data { get; init; }

    public required SavedEventLocation Location { get; init; }
}
