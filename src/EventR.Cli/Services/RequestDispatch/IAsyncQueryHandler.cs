namespace EventR.Cli.Services.RequestDispatch;

public interface IAsyncQueryHandler<TQuery, TResult> where TQuery : IAsyncQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);

}
