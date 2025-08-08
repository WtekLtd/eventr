namespace EventR.Cli.DTOs;

public record EndpointDataColumnDto
{
    public required string Title { get; set; }

    public required string Pointer { get; set; }
}
