using System.ComponentModel.DataAnnotations;

namespace EventR.Transports.Amazon.SQS.Source;

public record SQSEventSourceOptions
{
    [Range(1, 20)]
    public int? MaxNumberOfMessages { get; set; }

    [Required]
    [Url]
    public string? QueueUrl { get; set; }

    [Range(1, int.MaxValue)]
    public int? VisibilityTimeout { get; set; }

    [Range(0, int.MaxValue)]
    public int? WaitTimeSeconds { get; set; }
}
