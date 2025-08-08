using EventR.Cli.Constants;
using EventR.Cli.DTOs;
using EventR.Cli.Messages;
using EventR.Cli.Models;
using EventR.Cli.Services.RequestDispatch;
using EventR.Cli.Services.Messaging;
using EventR.Cli.Services;
using EventR.Cli.Services.Repositories;

namespace EventR.Cli.Requests.Commands.PublishEndpointEvent;

public class PublishEndpointEventCommandHandler(
    IRepository<Models.Endpoint> endpointRepository,
    IRepository<EndpointEvent> endpointEventRepository,
    IEventQueue eventQueue,
    IMessengerService messengerService
) : ICommandHandler<PublishEndpointEventCommand>
{
    public void Handle(PublishEndpointEventCommand command)
    {
        var model = new EndpointEvent()
        {
            Identifier = Guid.NewGuid().ToString(),
            Data = command.Data,
            DateTime = DateTime.UtcNow,
            Message = command.Message,
            Endpoint = endpointRepository.GetByIdentifier(command.EndpointIdentifier),
            Status = command.Status
        };

        endpointEventRepository.Save(model);

        if (command.Status == EventStatus.Sent)
        {
            eventQueue.Enqueue(command.EndpointIdentifier, model.Identifier);
        }

        messengerService.SendMessage(new EventPublishedMessage
        {
            EndpointEvent = EndpointEventDto.FromModel(model)
        });
    }
}
