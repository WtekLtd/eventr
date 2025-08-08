namespace EventR.Contracts;

public interface IEventProducer
{
    Task ProduceEventsAsync(IEnumerable<ProducedEvent> events, CancellationToken cancellationToken);
}
