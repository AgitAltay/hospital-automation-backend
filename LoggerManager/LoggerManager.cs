using LoggerManager.Interface;
using Microsoft.Extensions.Configuration;
using NLog;

namespace LoggerManager;

public class LoggerManager : ILoggerManager
{
    private static ILogger _logger = LogManager.GetCurrentClassLogger();

    public LoggerManager()
    {
        
    }
    public void LogInfo(string message) => _logger.Info(message);
    public void LogError(string message) => _logger.Error(message);
    public void LogWarn(string message)  => _logger.Warn(message);
    public void LogDebug(string message) => _logger.Debug(message);
}