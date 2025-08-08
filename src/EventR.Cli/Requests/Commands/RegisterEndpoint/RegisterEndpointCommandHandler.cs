using EventR.Cli.Constants;
using EventR.Cli.DTOs;
using EventR.Cli.Messages;
using EventR.Cli.Models;
using EventR.Cli.Services;
using EventR.Cli.Services.Messaging;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Requests.Commands.RegisterEndpoint;

public class RegisterEndpointCommandHandler(
    IRepository<Models.Endpoint> endpointRepository,
    IRepository<SavedEventLocation> savedEventLocationRepository,
    ISavedEventRepository savedEventRepository,
    IMessengerService messengerService,
    IEndpointStatusMonitor endpointStatusMonitor
) : ICommandHandler<RegisterEndpointCommand>
{
    public void Handle(RegisterEndpointCommand command)
    {
        var endpoint = endpointRepository.GetByIdentifier(command.Identifier);
        if (endpoint == null)
        {
            endpoint = new Models.Endpoint
            {
                Columns = [..command.Columns.Select(c => new EndpointColumn
                {
                    Pointer = c.Pointer,
                    Title = c.Title
                })],
                Identifier = command.Identifier,
                LastConnectedTime = DateTime.UtcNow,
                Status = EndpointStatus.Running,
                Name = command.Name
            };

            endpointRepository.Save(endpoint);
            messengerService.SendMessage(new EndpointRegisteredMessage
            {
                Endpoint = EndpointDto.FromModel(endpoint)
            });

            var location = new SavedEventLocation
            {
                Identifier = Guid.NewGuid().ToString(),
                Name = command.Name,
                Path = command.SavedEventPath
            };
            savedEventLocationRepository.Save(location);

            var savedEvents = savedEventRepository.LoadLocation(location);
            foreach (var savedEvent in savedEvents)
            {
                messengerService.SendMessage(new EventLoadedMessage
                {
                    SavedEvent = SavedEventDto.FromModel(savedEvent)
                });
            }
        }
        else
        {
            endpoint = endpoint with
            {
                LastConnectedTime = DateTime.UtcNow,
                Status = EndpointStatus.Running
            };
            endpointRepository.Save(endpoint);

            messengerService.SendMessage(new EndpointStatusChangedMessage
            {
                EndpointIdentifier = endpoint.Identifier,
                Status = EndpointStatus.Running
            });
        }

        endpointStatusMonitor.Start();
    }
}
