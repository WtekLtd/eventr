namespace EventR.Cli.Interface.Requests;

public record PublishEventRequest
{
    public required string Data { get; init; }
}
