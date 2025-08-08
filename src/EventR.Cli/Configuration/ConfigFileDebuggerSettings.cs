using System.Text.Json.Serialization;

namespace EventR.Cli.Configuration;

public class ConfigFileDebuggerSettings
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? DefaultPort { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TimeoutMilliseconds { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<SharedEventFolder>? SharedEventFolders { get; set; }
}