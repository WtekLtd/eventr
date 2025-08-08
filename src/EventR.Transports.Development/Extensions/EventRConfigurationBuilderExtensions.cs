using System.Threading.Channels;
using EventR.Cli.Interface.Requests;
using EventR.Contracts;
using EventR.Transports.Development;
using EventR.Transports.Development.Configuration;
using EventR.Transports.Development.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventR;

public static class EventRConfigurationBuilderExtensions
{
    public static EventRConfigurationBuilder UseDebugger(this EventRConfigurationBuilder builder, Action<DevelopmentTransportOptions> configure)
    {
        AddDefaultServices(builder);
        builder.Services.Configure(configure);

        return builder;
    }

    public static void UseDebugger(this EventRConfigurationBuilder builder, IConfiguration config)
    {
        AddDefaultServices(builder);
        builder.Services.Configure<DevelopmentTransportOptions>(config);
    }

    private static void AddDefaultServices(EventRConfigurationBuilder builder)
    {
        var publishLogRequestChannel = Channel.CreateUnbounded<PublishLogRequest>();

        builder.Services.AddSingleton<ILoggerProvider, DebugLoggerProvider>();
        builder.Services.AddSingleton(publishLogRequestChannel.Reader);
        builder.Services.AddSingleton(publishLogRequestChannel.Writer);
        builder.Services.AddHostedService<LogWriterService>();

        builder.Services.AddSingleton<IEventSource, DebugEventSource>();
        builder.Services.AddSingleton<IEventProducer, DebugEventProducer>();
        builder.Services.AddSingleton<IDebugServiceClient, DebugServiceClient>();
        builder.Services.AddHttpClient(DebugServiceClient.ClientName, (services, client) =>
        {
            var config = services.GetRequiredService<IOptions<DevelopmentTransportOptions>>();
            client.BaseAddress = new Uri($"http://localhost:{config.Value.Port}/api/");
        }).RemoveAllLoggers();
    }
}