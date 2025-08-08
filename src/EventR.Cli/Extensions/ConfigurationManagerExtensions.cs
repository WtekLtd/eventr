using System.Reflection;

namespace EventR.Cli.Extensions;

public static class ConfigurationManagerExtensions
{
    public static void AddEventRConfigurationFiles(this ConfigurationManager configuration)
    {
        var globalPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)!;

        var globalConfigfile = Path.Combine(globalPath, "eventr.config.json");
        configuration.AddJsonFile(globalConfigfile, optional: true);

        var userProfileDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var userConfigFile = Path.Combine(userProfileDir, "eventr.config.json");
        configuration.AddJsonFile(userConfigFile, optional: true);
    }
}