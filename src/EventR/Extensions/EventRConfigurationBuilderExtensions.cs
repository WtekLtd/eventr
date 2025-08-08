using EventR.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace EventR;

public static class EventRConfigurationBuilderExtensions
{
    public static EventRConfigurationBuilder AddEventProcessor<TProcessor>(this EventRConfigurationBuilder builder)
        where TProcessor : class, IEventProcessor
    {
        builder.Services.AddSingleton<IEventProcessor, TProcessor>();

        return builder;   
    }
}