namespace EventR.Cli.Interface.Requests;

public record RegisterEndpointRequest
{
    public required string Identifier { get; init; }

    public required string Name { get; init; }

    public required string SavedEventPath { get; init; }

    public required ICollection<EndpointDataColumnRequest> Columns { get; init; }
}
