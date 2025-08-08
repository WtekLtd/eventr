namespace EventR.Cli.Services.Messaging;

public delegate void MessageReceivedEventHandler<TMessage>(TMessage message);

public interface IMessenger
{
    public Type MessageType { get; }
}

public interface IMessenger<TMessage> : IMessenger
{
    public event MessageReceivedEventHandler<TMessage>? MessageReceived;

    public void SendMessage(TMessage message);
}