using System.CommandLine;
using EventR;
using EventR.Lambda.Launcher.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var portOption = new Option<int?>(
    name: "--port",
    description: "The port at which the debugger is hosted."
);

var identifierOption = new Option<string?>(
    name: "--identifier",
    description: "The unique identifier for this service (defaults to the assembly name)"
);

var nameOption = new Option<string?>(
    name: "--name",
    description: "The descriptive name of this service (defaults to the assembly name)"
);

var assemblyArgument = new Argument<string>(
    name: "binary",
    description: "The binary that is debugged."
);

var rootCommand = new RootCommand("EventR Lambda Launcher.")
{
    portOption,
    identifierOption,
    nameOption,
    assemblyArgument
};

rootCommand.SetHandler(
    RunLambdaAsync,
    portOption,
    identifierOption,
    nameOption,
    assemblyArgument
);

await rootCommand.InvokeAsync(args);

async Task RunLambdaAsync(int? port, string? identifier, string? name, string assemblyPath)
{
    var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
    var assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);

    var builder = Host.CreateApplicationBuilder(args);

    builder.Configuration.AddJsonFile("./aws-lambda-tools-defaults.json");

    builder.Services.Configure<LambdaEventProcessorOptions>((options) =>
    {
        var assemblyLoadContext = new LambdaAssemblyLoadContext(assemblyPath);

        options.Assembly = assemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
        options.FunctionHandler = builder.Configuration.GetValue<string>("function-handler");
        options.FunctionName = builder.Configuration.GetValue<string>("function-name");
        options.FunctionVersion = builder.Configuration.GetValue<string>("function-version");
        options.MaxMemorySize = builder.Configuration.GetValue<int>("function-memory-size");
    });

    builder.Services.AddEventR(eventr =>
    {
        eventr.UseDebugger(config =>
        {
            config.Port = port ?? 5050;
            config.Identifier = identifier ?? assemblyName;
            config.Name = name ?? assemblyName;
            config.SavedEventPath = ".eventr";
        });
        eventr.AddEventProcessor<LambdaEventProcessor>();
    });

    var app = builder.Build();

    await app.RunAsync();
}