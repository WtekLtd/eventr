namespace EventR.Cli.Interface.Requests;

public record UpdateEventStatusRequest
{
    public required bool IsSuccess { get; init; }

    public string? Message { get; init; }
}
