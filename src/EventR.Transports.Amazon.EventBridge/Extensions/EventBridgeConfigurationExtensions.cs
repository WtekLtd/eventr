using Amazon.EventBridge;
using EventR.Contracts;
using EventR.Transports.Amazon.EventBridge;
using EventR.Transports.Amazon.EventBridge.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventR;

public static class EventBridgeConfigurationExtensions
{
    public static EventREventBridgeConfigurationBuilder UseAmazonEventBridge(this EventRConfigurationBuilder builder)
    {
        builder.Services.AddAWSService<IAmazonEventBridge>();
        return new(builder.Services);
    }

    public static EventREventBridgeConfigurationBuilder AsProducer(this EventREventBridgeConfigurationBuilder builder, IConfiguration config)
    {
        builder.Services
            .AddOptions<EventBridgeEventProducerOptions>()
            .Bind(config)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.AddEventBridgeProducerServices();
        return builder;
    }

    public static EventREventBridgeConfigurationBuilder AsProducer(this EventREventBridgeConfigurationBuilder builder, Action<EventBridgeEventProducerOptions> configure)
    {
        builder.Services
            .AddOptions<EventBridgeEventProducerOptions>()
            .Configure(configure)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.AddEventBridgeProducerServices();
        return builder;
    }

    private static void AddEventBridgeProducerServices(this EventREventBridgeConfigurationBuilder builder)
    {
        builder.Services.AddSingleton<IEventProducer, EventBridgeEventProducer>();
    }
}
