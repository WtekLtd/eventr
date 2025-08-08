using EventR.Cli.Constants;
using EventR.Cli.Messages;
using EventR.Cli.Services.Messaging;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Commands.PingEndpoint;

public class PingEndpointCommandHandler(
    IRepository<Models.Endpoint>  endpointRepository,
    IMessengerService messengerService
) : ICommandHandler<PingEndpointCommand>
{
    public void Handle(PingEndpointCommand command)
    {
        var endpoint = endpointRepository.GetByIdentifier(command.EndpointIdentifier);
        if (endpoint != null)
        {
            endpointRepository.Save(endpoint with { LastConnectedTime = DateTime.UtcNow, Status = EndpointStatus.Running });
            if (endpoint.Status == EndpointStatus.Stopped)
            {
                messengerService.SendMessage(new EndpointStatusChangedMessage
                {
                    EndpointIdentifier = command.EndpointIdentifier,
                    Status = EndpointStatus.Running
                });
            }
        }
    }
}