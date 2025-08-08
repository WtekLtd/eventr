namespace EventR.Contracts;

public interface IEventSource
{
    Task SubscribeAsync(CancellationToken cancellationToken);

    Task UnsubscribeAsync(CancellationToken cancellationToken);

    Task<ConsumedEvent?> GetEventAsync(CancellationToken cancellationToken);

    Task FinalizeEventAsync(ConsumedEvent evnt, CancellationToken cancellationToken);

    Task FailEventAsync(ConsumedEvent evnt, string message, CancellationToken cancellationToken);
}
