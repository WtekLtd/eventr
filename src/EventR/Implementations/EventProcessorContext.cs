using EventR.Contracts;

namespace EventR.Implementations;

public class EventProcessorContext(ConsumedEvent incomingEvent) : IEventProcessorContext
{
    public ConsumedEvent IncomingEvent { get; } = incomingEvent;

    public ICollection<ProducedEvent> OutgoingEvents { get; } = [];
}
