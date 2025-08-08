using System.Data;
using EventR.Cli.Configuration;
using EventR.Cli.Constants;
using EventR.Cli.Messages;
using EventR.Cli.Services.Messaging;
using EventR.Cli.Services.Repositories;
using Microsoft.Extensions.Options;

namespace EventR.Cli.Services;

public class EndpointStatusMonitor(
    IRepository<Models.Endpoint> endpointRepository,
    IOptions<DebuggerSettings> clientSettings,
    IMessengerService messengerService
) : IEndpointStatusMonitor
{
    private Timer? _timer;

    public void Start()
    {
        _timer ??= new Timer(
            callback: OnStatusCheckTimerTick,
            state: null,
            dueTime: 1000,
            period: Timeout.Infinite);
    }

    private void OnStatusCheckTimerTick(object? state)
    {
        var currentTime = DateTime.UtcNow;
        var staleTime = currentTime.AddMilliseconds(-clientSettings.Value.TimeoutMilliseconds + 1000);
        var endpoints = endpointRepository.GetAll().Where(ep => ep.Status == EndpointStatus.Running);
        foreach (var endpoint in endpoints)
        {
            if (endpoint.LastConnectedTime < staleTime)
            {
                messengerService.SendMessage<EndpointStatusChangedMessage>(new()
                {
                    EndpointIdentifier = endpoint.Identifier,
                    Status = EndpointStatus.Stopped
                });
                endpointRepository.Save(endpoint with { Status = EndpointStatus.Stopped });
            }
        }

        _timer?.Change(1000, Timeout.Infinite);
    }
}
