using EventR.Cli.Models;

namespace EventR.Cli.DTOs;

public record SavedEventDto
{
    public required string Identifier { get; init; }

    public required string Name { get; init; }

    public required string Data { get; init; }

    public required string LocationIdentifier { get; init; }

    public required string LocationName { get; init; }

    public static SavedEventDto FromModel(SavedEvent model) => new()
    {
        Identifier = model.Identifier,
        Data = model.Data,
        LocationIdentifier = model.Location.Identifier,
        LocationName = model.Location.Name,
        Name = model.Name
    };
}
