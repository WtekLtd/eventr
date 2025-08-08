namespace EventR.Cli.Services.RequestDispatch;

public interface IRequestDispatcher
{
    void DispatchCommand<TCommand>(TCommand command) where TCommand : ICommand;

    Task DispatchCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : IAsyncCommand;

    TResult DispatchQuery<TResult>(IQuery<TResult> query);

    Task<TResult> DispatchQueryAsync<TResult>(IAsyncQuery<TResult> query, CancellationToken cancellationToken);
}
