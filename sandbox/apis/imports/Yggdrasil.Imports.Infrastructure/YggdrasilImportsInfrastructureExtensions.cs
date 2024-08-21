namespace Yggdrasil;

using Yggdrasil.Host.Abstractions;

/// <summary>
///   Extension methods to simplify
/// </summary>
public static class YggdrasilImportsInfrastructureExtensions {
  /// <summary>
  ///   The function to use when including Infrastructure dependencies.
  /// </summary>
  /// <param name="host">The host to register dependencies with.</param>
  /// <typeparam name="T">This should be the concrete host type: CRTP.</typeparam>
  /// <returns>The host.</returns>
  public static T UseImportsInfrastructure<T>(this T host) where T : IYggdrasilHost<T> {
    return host.ConfigureServices(new ImportsInfrastructureRegistry());
  }
}