using System.ComponentModel.DataAnnotations;

namespace EventR.Transports.Amazon.EventBridge.Producer;

public class EventBridgeEventProducerOptions
{
    public string? EndpointId { get; set; }

    [Required]
    public string? EventBusName { get; set; }
}
