namespace Contracts;

/// <summary>
/// The logging manager contract.
/// </summary>
public interface ILoggerManager
{
    /// <summary>
    /// Logs the information message.
    /// </summary>
    /// <param name="message">The information message.</param>
    void LogInfo(string message);
    /// <summary>
    /// Logs the warning message.
    /// </summary>
    /// <param name="message">The warning message.</param>
    void LogWarning(string message);
    /// <summary>
    /// Logs the debug message.
    /// </summary>
    /// <param name="message">The debug message.</param>
    void LogDebug(string message);
    /// <summary>
    /// Logs the error message.
    /// </summary>
    /// <param name="message">The error message.cdcd</param>
    void LogError(string message);
}
