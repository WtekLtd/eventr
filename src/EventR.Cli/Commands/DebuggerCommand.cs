using System.CommandLine;
using System.Diagnostics;
using Brism;
using EventR.Cli.Components;
using EventR.Cli.Configuration;
using EventR.Cli.Extensions;
using EventR.Cli.Middleware;
using EventR.Cli.Models;
using EventR.Cli.Services;
using EventR.Cli.Services.Messaging;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;
using MudBlazor.Services;

namespace EventR.Cli.Commands;

public static class DebuggerCommand
{
    public static void AddDebuggerCommand(this RootCommand rootCommand)
    {
        var command = new Command("debugger", "Controls the eventR debugger service.");

        command.AddDebuggerStartCommand();

        rootCommand.Add(command);
    }

    private static void AddDebuggerStartCommand(this Command debuggerCommand)
    {
        var portOption = new Option<int?>(
            name: "--port",
            description: "The port at which the debugger is hosted."
        );

        var launchBrowserOption = new Option<bool>(
            name: "--launch-browser",
            description: "If set to true, launches the browser automatically.",
            getDefaultValue: () => true
        );

        var command = new Command(
            name: "start",
            description: "Starts the eventR debugger service."
        )
        {
            portOption,
            launchBrowserOption
        };
        command.SetHandler(
            StartDebugger,
            portOption,
            launchBrowserOption
        );

        debuggerCommand.Add(command);
    }

    private static async Task StartDebugger(int? port, bool launchBrowser)
    {
        var currentAssembly = typeof(DebuggerCommand).Assembly;
        var currentAssemblyPath = Path.GetDirectoryName(currentAssembly.Location)!;
        var toolContentRootPath = Path.Combine(currentAssemblyPath, "../../../content");
        var contentRootPath = Path.Exists(toolContentRootPath)
            ? toolContentRootPath
            : currentAssemblyPath;

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ContentRootPath = contentRootPath,
            Args = [],
        });

        // Configure additional config files.
        builder.Configuration.AddEventRConfigurationFiles();

        // Bind Debugger Settings.
        var debuggerConfigurationSection = builder.Configuration.GetSection("Debugger");
        builder.Services.Configure<DebuggerSettings>(debuggerConfigurationSection);
        var debuggerSettings = debuggerConfigurationSection.Get<DebuggerSettings>();

        // Configure URL.
        var portNumber = port ?? debuggerSettings?.DefaultPort;
        var url = $"http://localhost:{portNumber}";
        builder.WebHost.UseUrls(url);

        // Register Third Party Services.
        builder.Services.AddMudServices();
        builder.Services.AddBrism();
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Register Local Services.
        builder.Services.AddSingleton<IRepository<Models.Endpoint>, TransitoryRepository<Models.Endpoint>>();
        builder.Services.AddSingleton<IRepository<EndpointEvent>, TransitoryRepository<EndpointEvent>>();
        builder.Services.AddSingleton<ISavedEventRepository, SavedEventRepository>();
        builder.Services.AddSingleton<IRepository<SavedEventLocation>, SavedEventLocationRepository>();
        builder.Services.AddSingleton<IEventQueue, EventQueue>();
        builder.Services.AddSingleton<IMessengerService, MessengerService>();
        builder.Services.AddSingleton<IEndpointStatusMonitor, EndpointStatusMonitor>();
        builder.Services.AddRequestDispatcher(currentAssembly);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseAntiforgery();
        app.UseMiddleware<PingEndpointMiddleware>();

        // Configure API Endpoints.
        app.MapRegisterEndpoint();
        app.MapDetachEndpoint();
        app.MapPublishEvent();
        app.MapPublishLog();
        app.MapDequeueEvent();
        app.MapUpdateEventStatus();

        app.UseStaticFiles();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        await app.StartAsync();

        if (launchBrowser)
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        await app.WaitForShutdownAsync();
    }
}