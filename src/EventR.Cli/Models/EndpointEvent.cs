using EventR.Cli.Constants;

namespace EventR.Cli.Models;

public record EndpointEvent : BaseModel
{
    public required Endpoint? Endpoint { get; init; }

    public required DateTime DateTime { get; init; }

    public required EventStatus Status { get; init; }

    public string? Message { get; init; }

    public required string Data { get; init; }
}
