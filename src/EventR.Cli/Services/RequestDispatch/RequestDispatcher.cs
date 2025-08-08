namespace EventR.Cli.Services.RequestDispatch;

public class RequestDispatcher(IServiceProvider serviceProvider) : IRequestDispatcher
{
    public void DispatchCommand<TCommand>(TCommand command) where TCommand : ICommand
    {
        using var scope = serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetService<ICommandHandler<TCommand>>();
        if (handler == null)
        {
            throw new InvalidOperationException($"No registered command handlers found for {typeof(TCommand)}");
        }
        handler.Handle(command);
    }

    public async Task DispatchCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : IAsyncCommand
    {
        using var scope = serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetService<IAsyncCommandHandler<TCommand>>();
        if (handler == null)
        {
            throw new InvalidOperationException($"No registered command handlers found for {typeof(TCommand)}");
        }
        await handler.HandleAsync(command, cancellationToken);
    }

    public TResult DispatchQuery<TResult>(IQuery<TResult> query)
    {
        using var scope = serviceProvider.CreateScope();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetService(handlerType);
        if (handler == null)
        {
            throw new InvalidOperationException($"No registered query handlers found for {query.GetType()} returning {typeof(TResult)}");
        }

        var handleMethod = handler.GetType().GetMethod("Handle");
        if (handleMethod == null)
        {
            throw new InvalidOperationException("Cannot find valid Handle method on handler.");
        }

        var result = (TResult)handleMethod.Invoke(handler, [query])!;

        return result;
    }

    public async Task<TResult> DispatchQueryAsync<TResult>(IAsyncQuery<TResult> query, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var handlerType = typeof(IAsyncQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"No registered async query handlers found for {query.GetType()} returning {typeof(TResult)}");
        }

        var handleAsyncMethod = handlerType.GetMethod("HandleAsync");
        if (handleAsyncMethod == null)
        {
            throw new InvalidOperationException("Cannot find valid HandleAsync method on handler.");
        }

        var result = (Task<TResult>)handleAsyncMethod.Invoke(handler, [query, cancellationToken])!;
        return await result;
    }
}