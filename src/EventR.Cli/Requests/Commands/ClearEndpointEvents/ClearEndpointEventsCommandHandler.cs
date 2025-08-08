using EventR.Cli.Messages;
using EventR.Cli.Models;
using EventR.Cli.Services.Messaging;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Commands.ClearEndpointEvents;

public class ClearEndpointEventsCommandHandler(
    IRepository<EndpointEvent> endpointEventRepository,
    IMessengerService messengerService
) : ICommandHandler<ClearEndpointEventsCommand>
{
    public void Handle(ClearEndpointEventsCommand command)
    {
        var endpointEvents = endpointEventRepository.GetAll()
            .Where(ee => command.EndpointIdentifier == null || command.EndpointIdentifier == ee.Endpoint?.Identifier);

        foreach (var endpointEvent in endpointEvents)
        {
            endpointEventRepository.Delete(endpointEvent.Identifier);
        }

        messengerService.SendMessage(new EventsClearedMessage { EndpointIdentifier = command.EndpointIdentifier });
    }
}