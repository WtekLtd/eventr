using EventR.Cli.Messages;
using EventR.Cli.Services.Messaging;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Commands.DeleteEvent;

public class DeleteEventCommandHandler(
    ISavedEventRepository savedEventRepository,
    IMessengerService messengerService
) : ICommandHandler<DeleteEventCommand>
{
    public void Handle(DeleteEventCommand command)
    {
        savedEventRepository.Delete(command.EventIdentifier);
        messengerService.SendMessage(new EventDeletedMessage()
        {
            EventIdentifier = command.EventIdentifier
        });
    }
}