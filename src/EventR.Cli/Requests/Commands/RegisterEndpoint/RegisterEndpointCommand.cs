using EventR.Cli.Services.RequestDispatch;
using EventR.Cli.Interface.Requests;

namespace EventR.Cli.Requests.Commands.RegisterEndpoint;

public record RegisterEndpointCommand : ICommand
{
    public required string Identifier { get; init; }

    public required string Name { get; init; }

    public required string SavedEventPath { get; init; }

    public required ICollection<EndpointDataColumnRequest> Columns { get; init; }
}
