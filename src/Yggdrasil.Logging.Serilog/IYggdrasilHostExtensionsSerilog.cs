namespace Yggdrasil;

using Microsoft.Extensions.Configuration;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

using Yggdrasil.Host.Abstractions;
using Yggdrasil.Logging.Abstractions;
using Yggdrasil.Logging.Serilog;

public static class IYggdrasilHostExtensionsSerilog {
  static AnsiConsoleTheme Sixteen { get; } = new(
    new Dictionary<ConsoleThemeStyle, string> {
      [ConsoleThemeStyle.Text]             = AnsiEscapeSequence.Unthemed,
      [ConsoleThemeStyle.SecondaryText]    = AnsiEscapeSequence.Unthemed,
      [ConsoleThemeStyle.TertiaryText]     = AnsiEscapeSequence.Unthemed,
      [ConsoleThemeStyle.Invalid]          = AnsiEscapeSequence.Yellow,
      [ConsoleThemeStyle.Null]             = AnsiEscapeSequence.Blue,
      [ConsoleThemeStyle.Name]             = AnsiEscapeSequence.Unthemed,
      [ConsoleThemeStyle.String]           = AnsiEscapeSequence.Cyan,
      [ConsoleThemeStyle.Number]           = AnsiEscapeSequence.Magenta,
      [ConsoleThemeStyle.Boolean]          = AnsiEscapeSequence.Blue,
      [ConsoleThemeStyle.Scalar]           = AnsiEscapeSequence.Green,
      [ConsoleThemeStyle.LevelVerbose]     = AnsiEscapeSequence.Unthemed,
      [ConsoleThemeStyle.LevelDebug]       = AnsiEscapeSequence.Bold,
      [ConsoleThemeStyle.LevelInformation] = AnsiEscapeSequence.BrightCyan,
      [ConsoleThemeStyle.LevelWarning]     = AnsiEscapeSequence.BrightYellow,
      [ConsoleThemeStyle.LevelError]       = AnsiEscapeSequence.BrightRed,
      [ConsoleThemeStyle.LevelFatal]       = AnsiEscapeSequence.BrightRed,
    });

  public static T UseSerilog<T>(this T host) where T : IYggdrasilHost<T> {
    return host.ConfigureServices(
                 (r, c) => {
                   YggdrasilLog.SetLogFactory(type => {
                     var config = CreateDefaultConfiguration(c);
                     return new YggdrasilSerilogLogger(config.CreateLogger().ForContext(type));
                   });

                   r.For<IYggdrasilLogger>().Use(
                     _ => {
                       var config = CreateDefaultConfiguration(c);
                       return new YggdrasilSerilogLogger(config.CreateLogger());
                     });

                   r.For(typeof(IYggdrasilLogger<>)).Use(typeof(YggdrasilSerilogLogger<>));
                 })
               .ConfigureLogger(
                 c => {
                   var config = CreateDefaultConfiguration(c);
                   return new YggdrasilSerilogLogger(config.CreateLogger());
                 });
  }

  static LoggerConfiguration CreateDefaultConfiguration(IConfiguration configuration) {
    var config = new LoggerConfiguration();

    if (Environment.UserInteractive) {
      config.WriteTo.Console(theme: Sixteen);
    }

    config.ReadFrom.Configuration(configuration);
    config.WriteTo.Trace();

    return config;
  }

  static class AnsiEscapeSequence {
    public const string Unthemed = "";
    public const string Reset = "\x1b[0m";
    public const string Bold = "\x1b[1m";
    public const string Black = "\x1b[30m";
    public const string Red = "\x1b[31m";
    public const string Green = "\x1b[32m";
    public const string Yellow = "\x1b[33m";
    public const string Blue = "\x1b[34m";
    public const string Magenta = "\x1b[35m";
    public const string Cyan = "\x1b[36m";
    public const string White = "\x1b[37m";
    public const string BrightBlack = "\x1b[30;1m";
    public const string BrightRed = "\x1b[31;1m";
    public const string BrightGreen = "\x1b[32;1m";
    public const string BrightYellow = "\x1b[33;1m";
    public const string BrightBlue = "\x1b[34;1m";
    public const string BrightMagenta = "\x1b[35;1m";
    public const string BrightCyan = "\x1b[36;1m";
    public const string BrightWhite = "\x1b[37;1m";
  }
}