using Amazon.SQS;
using Amazon.SQS.Model;
using EventR.Contracts;
using Microsoft.Extensions.Options;

namespace EventR.Transports.Amazon.SQS.Producer;

public class SQSEventProducer(
    IAmazonSQS amazonSQS,
    IOptions<SQSEventProducerOptions> options) : IEventProducer
{
    public async Task ProduceEventsAsync(IEnumerable<ProducedEvent> events, CancellationToken cancellationToken)
    {
        await amazonSQS.SendMessageBatchAsync(
            new SendMessageBatchRequest
            {
                QueueUrl = options.Value.QueueUrl,
                Entries = events
                    .Select(e => new SendMessageBatchRequestEntry
                    {
                        MessageBody = e.Data,
                    })
                    .ToList()
            },
            cancellationToken
        );
    }
}