using EventR.Cli.DTOs;
using EventR.Cli.Messages;
using EventR.Cli.Services.RequestDispatch;
using EventR.Cli.Services.Messaging;
using EventR.Cli.Services.Repositories;

namespace EventR.Cli.Requests.Commands.SaveEvent;

public class SaveEventCommandHandler(
    ISavedEventRepository savedEventRepository,
    IMessengerService messengerService
) : ICommandHandler<SaveEventCommand>
{
    public void Handle(SaveEventCommand command)
    {
        var savedEvent = savedEventRepository.GetByIdentifier(command.EventIdentifier);
        if (savedEvent != null)
        {
            var updatedEvent = savedEvent with { Data = command.Data };

            savedEventRepository.Save(updatedEvent);
            messengerService.SendMessage(new EventSavedMessage
            {
                SavedEvent = SavedEventDto.FromModel(updatedEvent)
            });
        }
    }
}