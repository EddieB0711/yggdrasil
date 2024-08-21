namespace Yggdrasil.Host.Abstractions;

using Lamar;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Yggdrasil.Logging.Abstractions;

/// <summary>
///   An event signaled when the host is going to being it's main process.
/// </summary>
/// <param name="Registry">The DI container registry</param>
public record OnStartEvent(ServiceRegistry Registry);

/// <summary>
///   The event signaled when the host has been successfully configured.
/// </summary>
/// <param name="Container">The DI container.</param>
/// <param name="Configuration">The host's configuration object.</param>
public record OnConfiguredEvent(IContainer Container, IConfiguration Configuration);

/// <summary>
///   The event signaled when the host is disposing.
/// </summary>
/// <param name="Container">The DI container.</param>
/// <param name="Configuration">The host's configuration object.</param>
public record OnDisposedEvent(IContainer Container, IConfiguration Configuration);

/// <summary>
///   The terminal interface for the host.
/// </summary>
public interface IYggdrasilHost : IAsyncDisposable, IDisposable { }

/// <summary>
///   The interface defining methods for setting up the host.
/// </summary>
/// <typeparam name="THost">This should be the concrete host type: CRTP.</typeparam>
public interface IYggdrasilHost<out THost> : IYggdrasilHost {
  /// <summary>
  ///   The method used to register actions for when the host is going to start it's main process.
  /// </summary>
  /// <param name="next">The action to register.</param>
  /// <returns>The concrete host.</returns>
  THost OnStart(Action<OnStartEvent> next);

  /// <summary>
  ///   The method used to register actions for when the host's DI container is configured.
  /// </summary>
  /// <param name="next">The action to register.</param>
  /// <returns>The concrete host.</returns>
  THost OnConfigured(Func<OnConfiguredEvent, ValueTask> next);

  /// <summary>
  ///   The method used to register actions for when the host is begin disposed.
  /// </summary>
  /// <param name="next">The action to register.</param>
  /// <returns>The concrete host.</returns>
  THost OnDisposed(Func<OnDisposedEvent, ValueTask> next);

  /// <summary>
  ///   The method used to register callbacks to configure the host's DI container.
  /// </summary>
  /// <param name="config">The callback to register</param>
  /// <returns>The concrete host.</returns>
  THost ConfigureServices(Action<ServiceRegistry, IConfiguration> config);

  /// <summary>
  ///   The method used to register a factory callback to configure the host's DI container.
  /// </summary>
  /// <param name="registryFactory">The factory callback to register.</param>
  /// <returns>The concrete host.</returns>
  THost ConfigureServices(Func<IConfiguration, ServiceRegistry> registryFactory);

  /// <summary>
  ///   The method used to register service registries with the host's DI container.
  /// </summary>
  /// <param name="registries">The collections of service registries.</param>
  /// <returns>The concrete host.</returns>
  THost ConfigureServices(params ServiceRegistry[] registries);

  /// <summary>
  ///   The method used to register a service registry with the host's DI container.
  /// </summary>
  /// <typeparam name="T">The type of service registry to add.</typeparam>
  /// <returns>The concrete host.</returns>
  THost ConfigureServices<T>() where T : ServiceRegistry, new();

  /// <summary>
  ///   The method used to register a configuration callback with a known priority.
  /// </summary>
  /// <param name="config">The callback to register.</param>
  /// <param name="priority">The priority of the callback.</param>
  /// <returns>The concrete host.</returns>
  THost ConfigureServices(Action<IServiceCollection, IConfiguration> config, int priority = 1);

  /// <summary>
  ///   The method used to set the factory function the host will use to create a logger instance during startup.
  /// </summary>
  /// <param name="config">The factory function.</param>
  /// <returns>The concrete host.</returns>
  THost ConfigureLogger(Func<IConfiguration, IYggdrasilLogger> config);
}

/// <summary>
///   The interface used to specify the host is "runnable".
///   Examples of runnable hosts: Console, Web API, Blazor.
///   Examples of non-runnable hosts: Xamarin, MAUI.
/// </summary>
/// <typeparam name="THost">This should be the concrete host type: CRTP.</typeparam>
/// <typeparam name="TBuilder">The builder type for the runnable app.</typeparam>
/// <typeparam name="TApp">The type of runnable app.</typeparam>
public interface IYggdrasilRunnableHost<out THost, out TBuilder, out TApp> : IYggdrasilHost<THost> {
  /// <summary>
  ///   The method used to register a configuration for the host's builder.
  /// </summary>
  /// <param name="config">The configuration callback to register.</param>
  /// <param name="priority">The priority of the callback.</param>
  /// <returns>The concrete host.</returns>
  THost ConfigureBuilder(Action<TBuilder, IConfiguration> config, int priority = 1);

  /// <summary>
  ///   The method used to register a configuration for the host's app.
  /// </summary>
  /// <param name="config">The configuration callback to register.</param>
  /// <param name="priority">The priority of the callback.</param>
  /// <returns>The concrete host.</returns>
  THost ConfigureHost(Action<TApp, IConfiguration> config, int priority = 1);

  /// <summary>
  ///   The method that calls the app's entry method.
  /// </summary>
  /// <param name="args">The arguments required for the applications.</param>
  /// <returns>The async result of running the application.</returns>
  ValueTask Run(object args);
}