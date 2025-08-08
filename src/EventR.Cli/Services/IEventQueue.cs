namespace EventR.Cli.Services;

public interface IEventQueue
{
    public Task<string?> DequeueAsync(string endpointIdentifier, int timeout, CancellationToken cancellationToken);

    public void Enqueue(string endpointIdentifier, string eventIdentifier);
}
