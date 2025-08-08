namespace EventR.Cli.Services.Messaging;

public interface IMessengerService
{
    public void SendMessage<TMessage>(TMessage message);

    public void Subscribe<TMessage>(MessageReceivedEventHandler<TMessage> handler);

    public void Unsubscribe<TMessage>(MessageReceivedEventHandler<TMessage> handler);
}

