using EventR.Cli.Interface.Constants;

namespace EventR.Cli.Interface.Requests;

public record LogData
{
    public required string Context { get; init; }    

    public required LogLevel LogLevel { get; init; }

    public required DateTime Date { get; init; }

    public required string Message { get; init; }

    public string? StackTrace { get; init; }
}
