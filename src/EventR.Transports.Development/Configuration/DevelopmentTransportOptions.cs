using System.Reflection;

namespace EventR.Transports.Development.Configuration;

public record DevelopmentTransportOptions
{
    public int Port { get; set; } = 5050;

    public required string Identifier { get; set; }

    public required string Name { get; set; }

    public string SavedEventPath { get; set; } = ".eventr";

    public ICollection<EndpointColumnConfiguration> Columns { get; set; } = [];
}
