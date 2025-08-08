namespace EventR.Contracts;

public interface IEventProcessorContext
{
    ConsumedEvent IncomingEvent { get; }

    ICollection<ProducedEvent> OutgoingEvents { get; }
}
