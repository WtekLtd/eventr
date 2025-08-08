using System.Threading.Channels;
using EventR.Cli.Interface.Requests;
using EventR.Transports.Development.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace EventR.Transports.Development.Logging;

public class LogWriterService(
    ChannelReader<PublishLogRequest> publishLogRequestChannelReader,
    IDebugServiceClient debugServiceClient,
    IOptions<DevelopmentTransportOptions> options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var publishLogRequest in publishLogRequestChannelReader.ReadAllAsync(stoppingToken))
        {
            await debugServiceClient.PublishLogAsync(
                options.Value.Identifier,
                publishLogRequest,
                stoppingToken
            );
        }
    }
}