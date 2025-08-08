namespace EventR.Cli.Interface.Requests;

public record EndpointDataColumnRequest
{
    public required string Title { get; init; }

    public required string Pointer { get; init; }
}
