using EventR.Cli.Constants;

namespace EventR.Cli.DTOs;

public record EndpointDto
{
    public required string Identifier { get; init; }

    public required string Name { get; init; }

    public required DateTime LastSpottedAt { get; init; }

    public required EndpointStatus Status { get; init; }

    public required EndpointDataColumnDto[] Columns { get; init; }

    public static EndpointDto FromModel(Models.Endpoint model) => new()
    {
        Identifier = model.Identifier,
        Name = model.Name,
        LastSpottedAt = model.LastConnectedTime,
        Status = model.Status,
        Columns =
        [
            ..model.Columns.Select(c => new EndpointDataColumnDto()
            {
                Title = c.Title,
                Pointer = c.Pointer
            })
        ]
    };
}
