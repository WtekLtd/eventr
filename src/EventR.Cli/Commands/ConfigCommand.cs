using System.CommandLine;
using System.Reflection;
using System.Text.Json;
using EventR.Cli.Configuration;

namespace EventR.Cli.Commands;

public static class ConfigCommand
{
    private static readonly Option<bool> GlobalOption = new("--global", "Determines whether to use the global config file.");

    public static void AddConfigCommand(this RootCommand rootCommand)
    {
        var configRoot = new Command("config", "Manage shared and user configuration.")
        {
            GlobalOption
        };
        configRoot.AddSetPortCommand();
        configRoot.AddGetPortCommand();

        configRoot.AddSetTimeoutCommand();
        configRoot.AddGetTimeoutCommand();

        configRoot.AddAddSharedFolderCommand();
        configRoot.AddRemoveSharedFolderCommand();
        configRoot.AddGetSharedFoldersCommand();

        rootCommand.Add(configRoot);
    }

    private static void AddSetPortCommand(this Command configCommand)
    {
        var portArgument = new Argument<int>("port", "The default port to use.");
        var command = new Command("set-port", "Sets the default port.")
        {
            portArgument
        };
        command.SetHandler((global, port) => SetConfigValue(global, (config) => config.Debugger.DefaultPort = port), GlobalOption, portArgument);

        configCommand.Add(command);
    }

    private static void AddGetPortCommand(this Command configCommand)
    {
        var command = new Command("get-port", "Gets the default port.");
        command.SetHandler(async (global) =>
        {
            var config = await GetConfig(global);
            Console.WriteLine(config?.Debugger.DefaultPort);
        }, GlobalOption);

        configCommand.Add(command);
    }

    private static void AddSetTimeoutCommand(this Command configCommand)
    {
        var timeoutArgument = new Argument<int>("timeout", "The timeout in milliseconds to use.");
        var command = new Command("set-timeout", "Sets the default timeout for dequeuing an event.")
        {
            timeoutArgument
        };
        command.SetHandler((global, timeout) => SetConfigValue(global, (config) => config.Debugger.TimeoutMilliseconds = timeout), GlobalOption, timeoutArgument);

        configCommand.Add(command);
    }

    private static void AddGetTimeoutCommand(this Command configCommand)
    {
        var command = new Command("get-timeout", "Gets the default timeout for dequeuing an event.");
        command.SetHandler(async (global) =>
        {
            var config = await GetConfig(global);
            Console.WriteLine(config?.Debugger.TimeoutMilliseconds);
        }, GlobalOption);

        configCommand.Add(command);
    }

    private static void AddAddSharedFolderCommand(this Command configCommand)
    {
        var pathArgument = new Argument<DirectoryInfo>("path", "The path to the new shared folder.");
        var nameOption = new Option<string?>("--name", "The optional name of the folder.");

        var command = new Command("add-shared-folder", "Adds a new shared events folder.")
        {
            pathArgument,
            nameOption
        };
        command.SetHandler(
            (global, path, name) => SetConfigValue(global, (config) =>
            {
                config.Debugger.SharedEventFolders ??= [];
                config.Debugger.SharedEventFolders.Add(new() { Name = name ?? path.Name, Path = path.FullName });
            }),
            GlobalOption,
            pathArgument,
            nameOption
        );
        configCommand.Add(command);
    }

    private static void AddRemoveSharedFolderCommand(this Command configCommand)
    {
        var nameArgument = new Argument<string>("name", "The name of the shared folder to remove.");

        var command = new Command("remove-shared-folder", "Removes a shared events folder.")
        {
            nameArgument
        };
        command.SetHandler(
            (global, name) => SetConfigValue(global, (config) =>
            {
                if (config.Debugger.SharedEventFolders != null)
                {
                    config.Debugger.SharedEventFolders = [
                        ..config.Debugger.SharedEventFolders.Where(sef => sef.Name != name)
                    ];
                    config.Debugger.SharedEventFolders = config.Debugger.SharedEventFolders.Count == 0 ? null : config.Debugger.SharedEventFolders;
                }
            }),
            GlobalOption,
            nameArgument
        );
        configCommand.Add(command);
    }

    private static void AddGetSharedFoldersCommand(this Command configCommand)
    {
        var command = new Command("get-shared-folders", "Gets a list of shared folders.");
        command.SetHandler(
            async (global) =>
            {
                var config = await GetConfig(global);
                foreach (var sharedFolder in config?.Debugger.SharedEventFolders ?? [])
                {
                    Console.WriteLine($"{sharedFolder.Name} = {sharedFolder.Path}");
                }
            }, GlobalOption);
    }

    private static async Task SetConfigValue(bool global, Action<ConfigFile> setValue)
    {
        var executablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var configLocation = global
            ? executablePath
            : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var targetConfigFilePath = Path.Combine(configLocation, "eventr.config.json");

        ConfigFile configFile;
        if (File.Exists(targetConfigFilePath))
        {
            using var sourceConfigFileStream = File.OpenRead(targetConfigFilePath);
            configFile = (await JsonSerializer.DeserializeAsync<ConfigFile>(sourceConfigFileStream))!;
        }
        else
        {
            configFile = new() { Debugger = new() };
        }

        setValue(configFile);

        using var targetConfigFileStream = File.OpenWrite(targetConfigFilePath);
        await JsonSerializer.SerializeAsync(targetConfigFileStream, configFile);
    }

    private static async Task<ConfigFile?> GetConfig(bool global)
    {
        var executablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var configLocation = global
            ? executablePath
            : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var targetConfigFilePath = Path.Combine(configLocation, "eventr.config.json");

        ConfigFile configFile;
        if (File.Exists(targetConfigFilePath))
        {
            using var sourceConfigFileStream = File.OpenRead(targetConfigFilePath);
            configFile = (await JsonSerializer.DeserializeAsync<ConfigFile>(sourceConfigFileStream))!;

            return configFile;
        }

        return null;
    }
}