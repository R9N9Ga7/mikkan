namespace Server.Logger;

public sealed class FileLoggerProvider(string filePath) : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(_filePath, categoryName);
    }

    public void Dispose() { }

    readonly string _filePath = filePath;
}
