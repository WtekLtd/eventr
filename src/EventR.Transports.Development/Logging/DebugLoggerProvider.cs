using System.Collections.Concurrent;
using System.Threading.Channels;
using EventR.Cli.Interface.Requests;
using EventR.Transports.Development.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventR.Transports.Development.Logging;

public sealed class DebugLoggerProvider(
    ChannelWriter<PublishLogRequest> publishLogRequestChannelWriter) : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, DebugLogger> _loggers = new();

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, (name) => new(name, publishLogRequestChannelWriter));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}