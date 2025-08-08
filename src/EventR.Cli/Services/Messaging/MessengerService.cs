using System.Collections.Concurrent;

namespace EventR.Cli.Services.Messaging;

public class MessengerService : IMessengerService
{
    private readonly ConcurrentDictionary<Type, IMessenger> _messengers = [];

    public void SendMessage<TMessage>(TMessage message)
    {
        if (_messengers.TryGetValue(typeof(TMessage), out var messenger) && messenger is IMessenger<TMessage> typedMessenger)
        {
            typedMessenger.SendMessage(message);
        }
    }

    public void Subscribe<TMessage>(MessageReceivedEventHandler<TMessage> handler)
    {
        var messenger = GetOrCreateMessenger<TMessage>();
        messenger.MessageReceived -= handler;
        messenger.MessageReceived += handler;
    }

    public void Unsubscribe<TMessage>(MessageReceivedEventHandler<TMessage> handler)
    {
        var messenger = GetOrCreateMessenger<TMessage>();
        messenger.MessageReceived -= handler;
    }

    private IMessenger<TMessage> GetOrCreateMessenger<TMessage>()
    {
        return (IMessenger<TMessage>)_messengers
            .GetOrAdd(typeof(TMessage), (type) => new Messenger<TMessage>());
    }
}

