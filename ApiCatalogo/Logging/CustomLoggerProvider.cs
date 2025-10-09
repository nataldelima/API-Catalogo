using System.Collections.Concurrent;

namespace ApiCatalogo.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    private readonly CustomLoggerProviderConfiguration loggerConfig;

    private readonly ConcurrentDictionary<string, CustomerLogger> loggers =
        new ConcurrentDictionary<string, CustomerLogger>();

    public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
    {
        loggerConfig = config;
    }


    public void Dispose()
    {
        loggers.Clear();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggerConfig));
    }
}