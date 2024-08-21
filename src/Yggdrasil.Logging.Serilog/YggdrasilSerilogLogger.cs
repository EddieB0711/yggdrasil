namespace Yggdrasil.Logging.Serilog;

using global::Serilog;

using Yggdrasil.Logging.Abstractions;

public class YggdrasilSerilogLogger : IYggdrasilLogger {
  readonly ILogger _logger;

  public YggdrasilSerilogLogger(ILogger logger) {
    _logger = logger;
  }

  public void Dispose() {
    if (_logger is IDisposable d) {
      d.Dispose();
    }
  }

  public void Log(YggdrasilLogLevel logLevel, string message, params object[] args) {
    switch (logLevel) {
      case YggdrasilLogLevel.Fatal:
        _logger.Fatal(message, args);
        break;
      case YggdrasilLogLevel.Error:
        _logger.Error(message, args);
        break;
      case YggdrasilLogLevel.Warning:
        _logger.Warning(message, args);
        break;
      case YggdrasilLogLevel.Information:
        _logger.Information(message, args);
        break;
      case YggdrasilLogLevel.Debug:
        _logger.Debug(message, args);
        break;
      case YggdrasilLogLevel.Verbose:
      default:
        _logger.Verbose(message, args);
        break;
    }
  }

  public void Log(YggdrasilLogLevel logLevel, Exception ex, string message, params object[] args) {
    switch (logLevel) {
      case YggdrasilLogLevel.Fatal:
        _logger.Fatal(ex, message, args);
        break;
      case YggdrasilLogLevel.Error:
        _logger.Error(ex, message, args);
        break;
      case YggdrasilLogLevel.Warning:
        _logger.Warning(ex, message, args);
        break;
      case YggdrasilLogLevel.Information:
        _logger.Information(ex, message, args);
        break;
      case YggdrasilLogLevel.Debug:
        _logger.Debug(ex, message, args);
        break;
      case YggdrasilLogLevel.Verbose:
      default:
        _logger.Verbose(ex, message, args);
        break;
    }
  }
}

public class YggdrasilSerilogLogger<T> : YggdrasilSerilogLogger, IYggdrasilLogger<T> {
  public YggdrasilSerilogLogger(ILogger logger) : base(logger.ForContext(typeof(T))) { }
}