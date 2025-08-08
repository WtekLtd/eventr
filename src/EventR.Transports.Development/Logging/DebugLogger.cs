using System.Threading.Channels;
using EventR.Cli.Interface.Requests;
using Microsoft.Extensions.Logging;

namespace EventR.Transports.Development.Logging;

public class DebugLogger(
    string name,
    ChannelWriter<PublishLogRequest> publishLogRequestChannelWriter) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;

    public bool IsEnabled(LogLevel logLevel) => true;

    public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);
        await publishLogRequestChannelWriter.WriteAsync(
            new()
            {
                Data = new()
                {
                    Context = name,
                    Date = DateTime.UtcNow,
                    LogLevel = (Cli.Interface.Constants.LogLevel)logLevel,
                    Message = message,
                    StackTrace = exception?.StackTrace
                },
                LogLevel = (Cli.Interface.Constants.LogLevel)logLevel,
                Message = message
            },
            default
        );
    }
}
