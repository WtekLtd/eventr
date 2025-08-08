using System.ComponentModel.DataAnnotations;

namespace EventR.Transports.Amazon.SQS.Producer;

public class SQSEventProducerOptions
{
    [Required]
    [Url]
    public string? QueueUrl { get; set; }
}
