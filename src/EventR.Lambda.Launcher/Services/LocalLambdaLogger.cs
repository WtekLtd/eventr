using Amazon.Lambda.Core;
using EventR.Transports.Development;

namespace EventR.Lambda.Launcher.Services;

public class LocalLambdaLogger(IDebugServiceClient debugServiceClient, string endpointIdentifier) : ILambdaLogger
{
    public async void Log(string message)
    {
        await debugServiceClient.PublishLogAsync(
            endpointIdentifier,
            new()
            {
                LogLevel = Cli.Interface.Constants.LogLevel.Information,
                Message = message,
                Data = new()
                {
                    Context = "",
                    Date = DateTime.UtcNow,
                    LogLevel = Cli.Interface.Constants.LogLevel.Information,
                    Message = message,
                    StackTrace = string.Empty
                }
            },
            CancellationToken.None
        );
    }

    public void LogLine(string message)
    {
        Log(message);
    }
}