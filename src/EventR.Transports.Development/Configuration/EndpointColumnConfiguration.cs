namespace EventR.Transports.Development.Configuration;

public record EndpointColumnConfiguration
{
    public required string Title { get; set; }

    public required string Pointer { get; set; }
}