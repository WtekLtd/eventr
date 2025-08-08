using System.Threading.Channels;
using Amazon.SQS;
using Amazon.SQS.Model;
using EventR.Contracts;
using Microsoft.Extensions.Options;

namespace EventR.Transports.Amazon.SQS.Source;

public class SQSEventSource(
    IAmazonSQS amazonSQS,
    IOptions<SQSEventSourceOptions> options) : IEventSource
{
    private Channel<ConsumedEvent>? _channel;

    private CancellationTokenSource? _consumerCancellationTokenSource;

    public Task FailEventAsync(ConsumedEvent evnt, string message, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task FinalizeEventAsync(ConsumedEvent evnt, CancellationToken cancellationToken)
    {
        var metadata = evnt.Metadata as SQSEventMetadata;
        if (metadata == null)
        {
            return;
        }
        await amazonSQS.DeleteMessageAsync(
            new DeleteMessageRequest
            {
                QueueUrl = options.Value.QueueUrl,
                ReceiptHandle = metadata.ReceiptHandle
            },
            cancellationToken
        );
    }

    public async Task<ConsumedEvent?> GetEventAsync(CancellationToken cancellationToken)
    {
        if (_channel == null)
        {
            return null;
        }
        return await _channel.Reader.ReadAsync(cancellationToken);
    }

    public Task SubscribeAsync(CancellationToken cancellationToken)
    {
        _channel = Channel.CreateBounded<ConsumedEvent>(options.Value.MaxNumberOfMessages ?? 1);

        _consumerCancellationTokenSource = new();
        cancellationToken.Register(() => _consumerCancellationTokenSource.Cancel());

        Task.Run(ReceiveMessagesAsync, _consumerCancellationTokenSource.Token);

        return Task.CompletedTask;
    }

    public Task UnsubscribeAsync(CancellationToken cancellationToken)
    {
        _consumerCancellationTokenSource?.Cancel();
        return Task.CompletedTask;
    }

    private async Task ReceiveMessagesAsync()
    {
        var cancellationToken = _consumerCancellationTokenSource!.Token;
        while (!cancellationToken.IsCancellationRequested)
        {
            if (await _channel!.Writer.WaitToWriteAsync(cancellationToken))
            {
                var capacity = options.Value.MaxNumberOfMessages ?? 1;
                var count = _channel.Reader.Count;

                try
                {
                    var receiveMessagesResponse = await amazonSQS.ReceiveMessageAsync(
                        new ReceiveMessageRequest
                        {
                            QueueUrl = options.Value.QueueUrl,
                            MaxNumberOfMessages = capacity - count,
                            VisibilityTimeout = options.Value.VisibilityTimeout,
                            WaitTimeSeconds = options.Value.WaitTimeSeconds
                        },
                        cancellationToken
                    );

                    foreach (var message in receiveMessagesResponse.Messages)
                    {
                        await _channel.Writer.WriteAsync(
                            new()
                            {
                                Data = message.Body,
                                Metadata = new SQSEventMetadata
                                {
                                    MessageId = message.MessageId,
                                    ReceiptHandle = message.ReceiptHandle
                                }
                            }
                        );
                    }
                }
                catch
                {
                    await Task.Delay(5_000, cancellationToken);
                }
            }
        }
    }
}
