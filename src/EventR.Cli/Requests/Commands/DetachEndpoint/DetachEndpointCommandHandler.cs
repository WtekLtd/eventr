using EventR.Cli.Messages;
using EventR.Cli.Services.Messaging;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Commands.DetachEndpoint;

public class DetachEndpointCommandHandler(
    IRepository<Models.Endpoint> endpointRepository,
    IMessengerService messengerService
) : ICommandHandler<DetachEndpointCommand>
{
    public void Handle(DetachEndpointCommand command)
    {
        var endpoint = endpointRepository.GetByIdentifier(command.EndpointIdentifier);
        if (endpoint != null)
        {
            var detachedEndpoint = endpoint with
            {
                LastConnectedTime = DateTime.MinValue,
                Status = Constants.EndpointStatus.Stopped
            };
            endpointRepository.Save(detachedEndpoint);
            messengerService.SendMessage(new EndpointStatusChangedMessage
            {
                EndpointIdentifier = detachedEndpoint.Identifier,
                Status = detachedEndpoint.Status
            });
        }
    }
}