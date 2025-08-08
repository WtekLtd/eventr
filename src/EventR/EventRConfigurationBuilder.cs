using Microsoft.Extensions.DependencyInjection;

namespace EventR;

public class EventRConfigurationBuilder(IServiceCollection services)
{
    public IServiceCollection Services { get; } = services;
}