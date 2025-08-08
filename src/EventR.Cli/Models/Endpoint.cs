using EventR.Cli.Constants;

namespace EventR.Cli.Models;

public record Endpoint : BaseModel
{
    public required string Name { get; init; }

    public required DateTime LastConnectedTime { get; init; }

    public required EndpointStatus Status { get; init; }

    public required ICollection<EndpointColumn> Columns { get; init; }
}
