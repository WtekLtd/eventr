using EventR.Cli.Interface.Constants;

namespace EventR.Cli.Interface.Requests;

public record PublishLogRequest
{
    public required LogData Data { get; init; }

    public required LogLevel LogLevel { get; init; }

    public string? Message { get; init; }
}
