namespace EventR.Cli.Services.Messaging;

public class Messenger<TMessage> : IMessenger<TMessage>
{
    public Type MessageType => typeof(TMessage);

    public event MessageReceivedEventHandler<TMessage>? MessageReceived;

    public void SendMessage(TMessage message)
    {
        MessageReceived?.Invoke(message);
    }
}

