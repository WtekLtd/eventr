using EventR.Cli.Constants;
using EventR.Cli.Messages;
using EventR.Cli.Models;
using EventR.Cli.Services.RequestDispatch;
using EventR.Cli.Services.Messaging;
using EventR.Cli.Services.Repositories;

namespace EventR.Cli.Requests.Commands.UpdateEndpointEventStatus;

public class UpdateEndpointEventStatusCommandHandler(
    IRepository<EndpointEvent> endpointEventRepository,
    IMessengerService messengerService
) : ICommandHandler<UpdateEndpointEventStatusCommand>
{
    public void Handle(UpdateEndpointEventStatusCommand command)
    {
        var endpointEvent = endpointEventRepository.GetByIdentifier(command.EventIdentifier);
        if (endpointEvent != null)
        {
            var modifiedEndpointEvent = endpointEvent with
            {
                Status = command.IsSuccess ? EventStatus.ProcessingSuccessful : EventStatus.ProcessingFailed,
                Message = command.Message
            };
            endpointEventRepository.Save(modifiedEndpointEvent);
            messengerService.SendMessage(new EventStatusChangedMessage
            {
                EventIdentifier = modifiedEndpointEvent.Identifier,
                Status = modifiedEndpointEvent.Status,
                Message = modifiedEndpointEvent.Message
            });
        }
    }
}
