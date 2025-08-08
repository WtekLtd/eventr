namespace EventR.Cli.Configuration;

public record SharedEventFolder
{
    public required string Name { get; init; }
    
    public required string Path { get; init; }
}