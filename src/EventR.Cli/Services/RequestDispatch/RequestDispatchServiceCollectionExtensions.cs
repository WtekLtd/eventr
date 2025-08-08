using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventR.Cli.Services.RequestDispatch;

public static class RequestDispatchServiceCollectionExtensions
{
    private static readonly Type[] HandlerInterfaceTypes = [
        typeof(ICommandHandler<>),
        typeof(IAsyncCommandHandler<>),
        typeof(IQueryHandler<,>),
        typeof(IAsyncQueryHandler<,>)
    ];

    public static IServiceCollection AddRequestDispatcher(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var handlerServiceDescriptors = assembly.DefinedTypes
            .SelectMany(t => t.GetInterfaces().Select(i => new { Class = t.AsType(), Interface = i }))
            .Where(t => t.Interface.IsGenericType && HandlerInterfaceTypes.Contains(t.Interface.GetGenericTypeDefinition()))
            .Select(t => new ServiceDescriptor(
                serviceType: t.Interface,
                implementationType: t.Class,
                lifetime: ServiceLifetime.Scoped
            ));

        serviceCollection
            .AddSingleton<IRequestDispatcher, RequestDispatcher>()
            .TryAddEnumerable(handlerServiceDescriptors);

        return serviceCollection;
    }
}