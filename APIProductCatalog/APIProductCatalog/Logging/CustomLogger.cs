
namespace APIProductCatalog.Logging;

public class CustomLogger : ILogger
{
    readonly string _loggerName;
    readonly CustomLoggerProviderConfiguration _loggerConfig;

    public CustomLogger(string loggerName, CustomLoggerProviderConfiguration loggerConfig)
    {
        _loggerName = loggerName;
        _loggerConfig = loggerConfig;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == _loggerConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
        WriteIntoTextFile(message);
    }

    private void WriteIntoTextFile(string message)
    {
        string pathLogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
        using (StreamWriter streamWriter = new StreamWriter(pathLogFile, true))
        {
            try
            {
                streamWriter.WriteLine(message);
                streamWriter.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
