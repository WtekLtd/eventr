using EventR.Cli.Constants;
using EventR.Cli.Models;

namespace EventR.Cli.DTOs;

public record EndpointEventDto
{
    public required string Identifier { get; init; }

    public required string EndpointIdentifier { get; init; }

    public required string EndpointName { get; init; }

    public required DateTime DateTime { get; init; }

    public required EventStatus Status { get; init; }

    public string? Message { get; init; }

    public required string Data { get; init; }

    public static EndpointEventDto FromModel(EndpointEvent model) => new()
    {
        Data = model.Data,
        DateTime = model.DateTime,
        EndpointIdentifier = model.Endpoint?.Identifier ?? "UNKNOWN",
        EndpointName = model.Endpoint?.Name ?? "UNKNOWN",
        Identifier = model.Identifier,
        Status = model.Status,
        Message = model.Message
    };
}
