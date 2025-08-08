using EventR.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace EventR;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventR(this IServiceCollection serviceCollection, Action<EventRConfigurationBuilder> configure)
    {
        serviceCollection.AddHostedService<EventRBackgroundService>();

        var builder = new EventRConfigurationBuilder(serviceCollection);
        configure(builder);

        if (!serviceCollection.Any(sd => sd.ServiceType == typeof(IEventSource)))
        {
            throw new InvalidOperationException("EventR has been configured without a valid event source.");
        }

        if (!serviceCollection.Any(sd => sd.ServiceType == typeof(IEventProducer)))
        {
            throw new InvalidOperationException("EventR has been configured without a valid event producer.");
        }

        if (!serviceCollection.Any(sd => sd.ServiceType == typeof(IEventProcessor)))
        {
            throw new InvalidOperationException("EventR has been configured without a valid event processor.");
        }

        return serviceCollection;
    }
}