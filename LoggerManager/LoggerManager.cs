using LoggerManager.Interface;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace LoggerManager;

public class LoggerManager : ILoggerManager
{
    private static ILogger _logger = Log.Logger;

    public LoggerManager()
    {
        
    }
    public void LogInfo(string message) => _logger.Information(message);
    public void LogError(string message) => _logger.Error(message);
    public void LogWarn(string message)  => _logger.Warning(message);
    public void LogDebug(string message) => _logger.Debug(message);
}