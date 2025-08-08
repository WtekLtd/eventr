namespace EventR.Cli.Configuration;

public record DebuggerSettings
{
    public required int DefaultPort { get; set; }

    public required int TimeoutMilliseconds { get; set; }

    public required ICollection<SharedEventFolder> SharedEventsFolders { get; set; } = [];
}
