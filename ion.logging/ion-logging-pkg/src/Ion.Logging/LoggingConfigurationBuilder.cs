using Serilog;

namespace Ion.Logging;

public sealed class LoggingConfigurationBuilder
{
    internal readonly IList<Action<LoggerConfiguration, IMicroService>> Sinks = new List<Action<LoggerConfiguration, IMicroService>>();
    internal Extension Extension { get; }

    internal LoggingConfigurationBuilder(Extension extension)
    {
        this.Extension = extension;
    }

    public LoggingConfigurationBuilder ToConsole()
    {
        Sinks.Add((logger, service) => logger.WriteTo.Async(@async => @async.Console()));

        return this;
    }
}
