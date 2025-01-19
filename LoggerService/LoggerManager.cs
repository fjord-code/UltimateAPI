using Contracts;
using NLog;

namespace LoggerService;

/// <summary>
/// The logger manager.
/// </summary>
public class LoggerManager : ILoggerManager
{
    /// <summary>
    /// A logger from the NLog library.
    /// </summary>
    private static ILogger logger = LogManager.GetCurrentClassLogger();

    public LoggerManager() { }

    /// <inheritdoc/>
    public void LogDebug(string message)
    {
        logger.Debug(message);
    }

    /// <inheritdoc/>
    public void LogError(string message)
    {
        logger.Error(message);
    }

    /// <inheritdoc/>
    public void LogInfo(string message)
    {
        logger.Info(message);
    }

    /// <inheritdoc/>
    public void LogWarning(string message)
    {
        logger.Warn(message);
    }
}
