namespace EventR.Cli.Models;

public record SavedEventLocation : BaseModel
{
    public required string Name { get; init; }

    public required string Path { get; init; }
}