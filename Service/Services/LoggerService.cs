using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

    public interface ILoggerService
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(Exception ex, string message);
    void LogCritical(string message);
}


public class LoggerService : ILoggerService
{
private readonly ILogger<LoggerService> _logger;

public LoggerService(ILogger<LoggerService> logger)
{
    _logger = logger;
}

// Método para loguear mensajes informativos
public void LogInfo(string message)
{
    _logger.LogInformation(message);
}

// Método para loguear advertencias
public void LogWarning(string message)
{
    _logger.LogWarning(message);
}

// Método para loguear errores
public void LogError(Exception ex, string message)
{
    _logger.LogError(ex, message);
}

// Método para loguear errores críticos
public void LogCritical(string message)
{
    _logger.LogCritical(message);
}
}
