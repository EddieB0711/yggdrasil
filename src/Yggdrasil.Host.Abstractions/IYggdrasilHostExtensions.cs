namespace Yggdrasil;

using Yggdrasil.Host.Abstractions;

/// <summary>
///   Extension methods to simplify configuration of the host.
/// </summary>
public static class IYggdrasilHostExtensions {
  /// <summary>
  ///   The method to use to register a synchronous callback for when the host has been configured.
  /// </summary>
  /// <param name="host">The host to register the configured callback with.</param>
  /// <param name="action">The action that will be called when the host is configured.</param>
  /// <typeparam name="THost">This should be the concrete host type: CRTP.</typeparam>
  /// <returns>The concrete host.</returns>
  public static THost OnConfigured<THost>(this THost host, Action<OnConfiguredEvent> action) where THost : IYggdrasilHost<THost> {
    return host.OnConfigured(
      e => {
        action(e);
        return ValueTask.CompletedTask;
      });
  }

  /// <summary>
  ///   The method to use to register a synchronous callback for when the host is disposing.
  /// </summary>
  /// <param name="host">The host to register the configured callback with.</param>
  /// <param name="action">The action that will be called when the host is disposing.</param>
  /// <typeparam name="THost">This should be the concrete host type: CRTP.</typeparam>
  /// <returns>The concrete host.</returns>
  public static THost OnDisposed<THost>(this THost host, Action<OnDisposedEvent> action) where THost : IYggdrasilHost<THost> {
    return host.OnDisposed(
      e => {
        action(e);
        return ValueTask.CompletedTask;
      });
  }
}