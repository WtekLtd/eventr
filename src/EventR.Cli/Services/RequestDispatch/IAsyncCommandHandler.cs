namespace EventR.Cli.Services.RequestDispatch;

public interface IAsyncCommandHandler<TCommand> where TCommand : IAsyncCommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}
