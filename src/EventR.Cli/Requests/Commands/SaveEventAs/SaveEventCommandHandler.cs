using EventR.Cli.DTOs;
using EventR.Cli.Messages;
using EventR.Cli.Models;
using EventR.Cli.Services.RequestDispatch;
using EventR.Cli.Services.Messaging;
using EventR.Cli.Services.Repositories;

namespace EventR.Cli.Requests.Commands.SaveEventAs;

public class SaveEventAsCommandHandler(
    ISavedEventRepository savedEventRepository,
    IRepository<SavedEventLocation> savedEventLocationRepository,
    IMessengerService messengerService
) : ICommandHandler<SaveEventAsCommand>
{
    public void Handle(SaveEventAsCommand command)
    {
        var location = savedEventLocationRepository.GetByIdentifier(command.LocationIdentifier);
        if (location == null)
        {
            throw new InvalidOperationException($"Location {command.LocationIdentifier} does not exist.");
        }

        var savedEvent = new SavedEvent
        {
            Data = command.Data,
            Identifier = Guid.NewGuid().ToString(),
            Name = command.Name,
            Location = location
        };
        savedEventRepository.Save(savedEvent);
        messengerService.SendMessage(new EventSavedMessage
        {
            SavedEvent = SavedEventDto.FromModel(savedEvent)
        });
    }
}