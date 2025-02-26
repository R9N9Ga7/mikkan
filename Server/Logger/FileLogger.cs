using System.Diagnostics;
using System.Globalization;

namespace Server.Logger;

public sealed class FileLogger(string filePath, string categoryName) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return default!;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        lock (_lock)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffff", CultureInfo.InvariantCulture);

            var content = $"""

            [{_categoryName}]
                {formatter(state, exception)}
            Timestamp: {timestamp}
            TraceId: {GetTraceId()}

            """;

            File.AppendAllText(_filePath, content);
        }
    }
    static string GetTraceId()
    {
        return Activity.Current?.Id ?? Guid.NewGuid().ToString();
    }

    readonly string _filePath = filePath;
    readonly string _categoryName = categoryName;

    readonly Lock _lock = new();
}
