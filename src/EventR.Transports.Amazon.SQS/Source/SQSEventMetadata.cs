namespace EventR.Transports.Amazon.SQS.Source;

public record SQSEventMetadata
{
    public required string ReceiptHandle { get; init; }

    public required string MessageId { get; init; }
}