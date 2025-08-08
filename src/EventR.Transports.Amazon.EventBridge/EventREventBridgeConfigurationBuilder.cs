using Microsoft.Extensions.DependencyInjection;

namespace EventR.Transports.Amazon.EventBridge;

public class EventREventBridgeConfigurationBuilder(IServiceCollection services)
    : EventRConfigurationBuilder(services);