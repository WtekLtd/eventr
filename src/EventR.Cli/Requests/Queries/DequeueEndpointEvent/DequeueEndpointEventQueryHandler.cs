using EventR.Cli.Configuration;
using EventR.Cli.Messages;
using EventR.Cli.Models;
using EventR.Cli.Services;
using EventR.Cli.Services.Messaging;
using EventR.Cli.Services.Repositories;
using EventR.Cli.Services.RequestDispatch;
using EventR.Cli.Interface.Responses;
using Microsoft.Extensions.Options;

namespace EventR.Cli.Requests.Queries.DequeueEndpointEvent;

public class DequeueEndpointEventQueryHandler(
    IRepository<EndpointEvent> endpointEventRepository,
    IEventQueue eventQueue,
    IMessengerService messengerService,
    IOptions<DebuggerSettings> clientSettings
) : IAsyncQueryHandler<DequeueEndpointEventQuery, GetEventResponse?>
{
    public async Task<GetEventResponse?> HandleAsync(DequeueEndpointEventQuery query, CancellationToken cancellationToken)
    {
        var eventIdentifier = await eventQueue.DequeueAsync(
            query.EndpointIdentifier,
            clientSettings.Value.TimeoutMilliseconds,
            cancellationToken);

        if (eventIdentifier == null)
        {
            return null;
        }

        var endpointEvent = endpointEventRepository.GetByIdentifier(eventIdentifier);
        if (endpointEvent == null)
        {
            return null;
        }

        var modifiedEndpointEvent = endpointEvent with { Status = Constants.EventStatus.Received };
        endpointEventRepository.Save(modifiedEndpointEvent);
        messengerService.SendMessage(new EventStatusChangedMessage
        {
            EventIdentifier = eventIdentifier,
            Status = modifiedEndpointEvent.Status,
            Message = null
        });

        return endpointEvent == null ? null : new GetEventResponse
        {
            Identifier = endpointEvent.Identifier,
            Data = endpointEvent.Data
        };
    }
}