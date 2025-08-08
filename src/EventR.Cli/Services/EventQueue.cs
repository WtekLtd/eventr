using System.Collections.Concurrent;

namespace EventR.Cli.Services;

public class EventQueue : IEventQueue
{
    private readonly ConcurrentDictionary<string, BlockingCollection<string>> _concurrentEvents = [];

    public Task<string?> DequeueAsync(string endpointIdentifier, int timeout, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            var events = _concurrentEvents.GetOrAdd(endpointIdentifier, (_) => []);
            if (events.TryTake(out var item, timeout, cancellationToken))
            {
                return item;
            }
            return null;
        }, cancellationToken);
    }

    public void Enqueue(string endpointIdentifier, string eventIdentifier)
    {
        var events = _concurrentEvents.GetOrAdd(endpointIdentifier, (_) => []);
        events.Add(eventIdentifier);
    }
}