namespace Yggdrasil.Logging.Abstractions;

public static class IYggdrasilLoggerExtensions {
  public static void Verbose(this IYggdrasilLogger log, string message, params object[] messageArgs) {
    log.Log(YggdrasilLogLevel.Verbose, message, messageArgs);
  }

  public static void Verbose(this IYggdrasilLogger log, Exception ex, string message, params object[] messageArgs) {
    log.Log(YggdrasilLogLevel.Verbose, ex, message, messageArgs);
  }

  public static void Information(this IYggdrasilLogger log, string message, params object[] messageArgs) {
    log.Log(YggdrasilLogLevel.Information, message, messageArgs);
  }

  public static void Information(this IYggdrasilLogger log, Exception ex, string message, params object[] messageArgs) {
    log.Log(YggdrasilLogLevel.Information, ex, message, messageArgs);
  }

  public static void Debug(this IYggdrasilLogger log, string message, params object[] messageArgs) {
    log.Log(YggdrasilLogLevel.Debug, message, messageArgs);
  }

  public static void Debug(this IYggdrasilLogger log, Exception ex, string message, params object[] messageArgs) {
    log.Log(YggdrasilLogLevel.Debug, ex, message, messageArgs);
  }

  public static void Warning(this IYggdrasilLogger log, string message, params object[] messageArgs) {
    log.Log(YggdrasilLogLevel.Warning, message, messageArgs);
  }

  public static void Warning(this IYggdrasilLogger log, Exception ex, string message, params object[] messageArgs) {
    log.Log(YggdrasilLogLevel.Warning, ex, message, messageArgs);
  }

  public static void Error(this IYggdrasilLogger log, string message, params object[] messageArgs) {
    log.Log(YggdrasilLogLevel.Error, message, messageArgs);
  }

  public static void Error(this IYggdrasilLogger log, Exception ex, string message, params object[] messageArgs) {
    log.Log(YggdrasilLogLevel.Error, ex, message, messageArgs);
  }

  public static void Fatal(this IYggdrasilLogger log, string message, params object[] messageArgs) {
    log.Log(YggdrasilLogLevel.Fatal, message, messageArgs);
  }

  public static void Fatal(this IYggdrasilLogger log, Exception ex, string message, params object[] messageArgs) {
    log.Log(YggdrasilLogLevel.Fatal, ex, message, messageArgs);
  }
}