using EventR.Cli.Models;

namespace EventR.Cli.DTOs;

public record SavedEventLocationDto
{
    public required string Identifier { get; init; }

    public required string Name { get; init; }

    public required string Path { get; init; }

    public static SavedEventLocationDto FromModel(SavedEventLocation model) => new()
    {
        Identifier = model.Identifier,
        Name = model.Name,
        Path = model.Path
    };
}