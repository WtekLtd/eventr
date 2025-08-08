namespace EventR.Contracts;

public interface IEventProcessor
{
    Task ProcessEventAsync(IEventProcessorContext context, CancellationToken cancellationToken);
}
