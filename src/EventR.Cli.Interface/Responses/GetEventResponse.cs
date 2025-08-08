namespace EventR.Cli.Interface.Responses;

public record GetEventResponse
{
    public required string Identifier { get; init; }

    public required string Data { get; init; }
}
