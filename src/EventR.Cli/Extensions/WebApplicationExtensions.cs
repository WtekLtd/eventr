using System.Text.Json;
using EventR.Cli.Constants;
using EventR.Cli.Requests.Commands.PublishEndpointEvent;
using EventR.Cli.Requests.Commands.RegisterEndpoint;
using EventR.Cli.Requests.Commands.UpdateEndpointEventStatus;
using EventR.Cli.Requests.Queries.DequeueEndpointEvent;
using EventR.Cli.Services.RequestDispatch;
using EventR.Cli.Interface.Requests;
using Microsoft.AspNetCore.Mvc;
using EventR.Cli.Requests.Commands.DetachEndpoint;

namespace EventR.Cli.Extensions;

public static class WebApplicationExtensions
{
    public static RouteHandlerBuilder MapRegisterEndpoint(this WebApplication app)
    {
        return app.MapPost("/api/endpoints", ([FromBody] RegisterEndpointRequest request, IRequestDispatcher requestDispatcher) =>
        {
            requestDispatcher.DispatchCommand(new RegisterEndpointCommand
            {
                Identifier = request.Identifier,
                Name = request.Name,
                SavedEventPath = request.SavedEventPath,
                Columns = request.Columns
            });
        });
    }

    public static RouteHandlerBuilder MapDetachEndpoint(this WebApplication app)
    {
        return app.MapDelete("/api/endpoints/{endpointIdentifier}", (string endpointIdentifier, IRequestDispatcher requestDispatcher) =>
        {
            requestDispatcher.DispatchCommand(new DetachEndpointCommand
            {
                EndpointIdentifier = endpointIdentifier
            });
        });
    }

    public static RouteHandlerBuilder MapPublishEvent(this WebApplication app)
    {
        return app.MapPost("/api/endpoints/{endpointIdentifier}/events", (string endpointIdentifier, [FromBody] PublishEventRequest request, IRequestDispatcher requestDispatcher) =>
        {
            requestDispatcher.DispatchCommand(new PublishEndpointEventCommand
            {
                EndpointIdentifier = endpointIdentifier,
                Data = request.Data,
                Message = null,
                Status = EventStatus.Published
            });
        });
    }

    public static RouteHandlerBuilder MapPublishLog(this WebApplication app)
    {
        return app.MapPost("/api/endpoints/{endpointIdentifier}/logs", (string endpointIdentifier, [FromBody] PublishLogRequest request, IRequestDispatcher requestDispatcher) =>
        {
            requestDispatcher.DispatchCommand(new PublishEndpointEventCommand
            {
                EndpointIdentifier = endpointIdentifier,
                Data = JsonSerializer.Serialize(request.Data),
                Message = request.Message,
                Status = EventStatus.LoggedTrace + (int)request.LogLevel
            });
        });
    }

    public static RouteHandlerBuilder MapDequeueEvent(this WebApplication app)
    {
        return app.MapGet("/api/endpoints/{endpointIdentifier}/events", async (string endpointIdentifier, IRequestDispatcher requestDispatcher, CancellationToken cancellationToken) =>
        {
            return await requestDispatcher.DispatchQueryAsync(new DequeueEndpointEventQuery
            {
                EndpointIdentifier = endpointIdentifier
            }, cancellationToken);
        });
    }

    public static RouteHandlerBuilder MapUpdateEventStatus(this WebApplication app)
    {
        return app.MapPut("/api/events/{eventIdentifier}", (string eventIdentifier, [FromBody] UpdateEventStatusRequest request, IRequestDispatcher requestDispatcher) =>
        {
            requestDispatcher.DispatchCommand(new UpdateEndpointEventStatusCommand
            {
                EventIdentifier = eventIdentifier,
                IsSuccess = request.IsSuccess,
                Message = request.Message
            });
        });
    }
}