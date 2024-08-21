namespace Yggdrasil.Logging.Abstractions;

public enum YggdrasilLogLevel {
  Verbose,
  Information,
  Debug,
  Warning,
  Error,
  Fatal,
}

public interface IYggdrasilLogger : IDisposable {
  void Log(YggdrasilLogLevel logLevel, string message, params object[] args);

  void Log(YggdrasilLogLevel logLevel, Exception ex, string message, params object[] args);
}

public interface IYggdrasilLogger<T> : IYggdrasilLogger { }