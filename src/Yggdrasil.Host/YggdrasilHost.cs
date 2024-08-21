namespace Yggdrasil.Host;

using Lamar;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Priority_Queue;

using Yggdrasil.Abstractions;
using Yggdrasil.Abstractions.Eventing;
using Yggdrasil.Extensions;
using Yggdrasil.Host.Abstractions;
using Yggdrasil.Host.Eventing;
using Yggdrasil.Logging.Abstractions;

public abstract class YggdrasilHost<THost> : YggdrasilDisposable, IYggdrasilHost<THost> where THost : YggdrasilHost<THost> {
  IContainer? _container;

  protected IYggdrasilEventAggregator EventAggregator { get; } = new YggdrasilEventAggregator();

  protected IYggdrasilSubscriptionManager SubscriptionManager { get; } = new YggdrasilSubscriptionManager();

  protected ServiceRegistry Registry { get; } = new();

  protected SimplePriorityQueue<Action> Registrations { get; } = new();

  protected IYggdrasilLogger Logger { get; set; }

  protected IConfiguration Configuration { get; set; }

  protected Func<IConfiguration, IYggdrasilLogger> LoggerFactory { get; set; }

  protected IContainer Container {
    get => _container ??= CreateContainer();
    set => _container = value;
  }

  public THost OnStart(Action<OnStartEvent> next) {
    var observable = EventAggregator.GetEvent<OnStartEvent>();
    var subscription = observable.Subscribe(next);

    SubscriptionManager.Add(subscription);

    return (THost)this;
  }

  public THost OnConfigured(Func<OnConfiguredEvent, ValueTask> next) {
    var observable = EventAggregator.GetEvent<OnConfiguredEvent>();
    var subscription = observable.SubscribeAsync(next);

    SubscriptionManager.Add(subscription);

    return (THost)this;
  }

  public THost OnDisposed(Func<OnDisposedEvent, ValueTask> next) {
    var observable = EventAggregator.GetEvent<OnDisposedEvent>();
    var subscription = observable.SubscribeAsync(next);

    SubscriptionManager.Add(subscription);

    return (THost)this;
  }

  public THost ConfigureServices(Action<ServiceRegistry, IConfiguration> config) {
    Registrations.Enqueue(() => config(Registry, Configuration), 1);
    return (THost)this;
  }

  public THost ConfigureServices(Func<IConfiguration, ServiceRegistry> registryFactory) {
    Registrations.Enqueue(() => Registry.IncludeRegistry(registryFactory(Configuration)), 1);
    return (THost)this;
  }

  public THost ConfigureServices(params ServiceRegistry[] registries) {
    Registrations.Enqueue(() => {
      foreach (var registry in registries) {
        Registry.IncludeRegistry(registry);
      }
    }, 1);

    return (THost)this;
  }

  public THost ConfigureServices<T>() where T : ServiceRegistry, new() {
    Registrations.Enqueue(Registry.IncludeRegistry<T>, 1);
    return (THost)this;
  }

  public THost ConfigureServices(Action<IServiceCollection, IConfiguration> config, int priority) {
    Registrations.Enqueue(() => config(Registry, Configuration), priority);
    return (THost)this;
  }

  public THost ConfigureLogger(Func<IConfiguration, IYggdrasilLogger> config) {
    LoggerFactory = config;
    return (THost)this;
  }

  protected virtual IContainer CreateContainer() {
    return new Container(Registry);
  }

  protected virtual ValueTask<IConfigurationBuilder> CreateConfigurationBuilder(object? args) {
    var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddEnvironmentVariables("ASPNETCORE_")
                 .AddJsonFile("appsettings.json", true)
                 .AddJsonFile("appsettings.Production.json", true)
                 .AddJsonFile("appsettings.Development.json", true)
                 .AddEnvironmentVariables();

    if (args is string[] strArgs) {
      config.AddCommandLine(strArgs);
    }

    return new(config);
  }

  protected virtual void ConfigureContainer() {
    while (Registrations.Count > 0) {
      Registrations.Dequeue()();
    }

    Registry.For<IConfiguration>().Use(Configuration);
    Registry.For<IYggdrasilEventAggregator>().Use<YggdrasilEventAggregator>().Singleton();
    Registry.For<IYggdrasilSubscriptionManager>().Use<YggdrasilSubscriptionManager>();
  }

  protected virtual IYggdrasilLogger CreateLogger() {
    return LoggerFactory(Configuration);
  }

  protected override async ValueTask OnDisposeAsync() {
    if (_container != null) {
      await _container.DisposeAsync().ConfigureAwait(false);
    }

    SubscriptionManager.Dispose();
    EventAggregator.Dispose();
    Logger.Dispose();

    await DisposeHost().ConfigureAwait(false);
  }

  protected abstract ValueTask DisposeHost();
}

public abstract class YggdrasilRunnableHost<THost, TBuilder, TApp> : YggdrasilHost<THost>, IYggdrasilRunnableHost<THost, TBuilder, TApp>
  where THost : YggdrasilRunnableHost<THost, TBuilder, TApp> {
  protected SimplePriorityQueue<Action> HostRegistrations { get; } = new();

  protected TBuilder Builder { get; set; }

  protected TApp? Host { get; set; }

  public virtual THost ConfigureBuilder(Action<TBuilder, IConfiguration> config, int priority = 1) {
    Registrations.Enqueue(() => config(Builder, Configuration), priority);
    return (THost)this;
  }

  public virtual THost ConfigureHost(Action<TApp, IConfiguration> config, int priority = 1) {
    HostRegistrations.Enqueue(() => config(Host, Configuration), priority);
    return (THost)this;
  }

  public async ValueTask Run(object args) {
    try {
      Builder       = CreateBuilder(args);
      Configuration = (await CreateConfigurationBuilder(args)).Build();
      Logger        = CreateLogger();

      Logger.Verbose("Configuring Container.");

      ConfigureContainer();

      Logger.Verbose("Configured Container.");
      Logger.Verbose("Creating Host.");

      Host = CreateHost();

      ConfigureHost();

      Logger.Verbose("Created Host.");
      Logger.Verbose("Publishing OnConfiguredEvent");

      EventAggregator.Publish(new OnConfiguredEvent(Container, Configuration));

      Logger.Verbose("Published OnConfiguredEvent");
    } catch (HostAbortedException) { } catch (Exception e) {
      Logger.Error(e, "An error occurred setting up host and container.");

      throw;
    }

    try {
      Logger.Verbose("Staring main process.");

      EventAggregator.Publish(new OnStartEvent(Registry));

      await StartHost().ConfigureAwait(false);
    } catch (OperationCanceledException) { } catch (Exception e) {
      Logger.Error(e, "Error running host.");

      throw;
    }

    Logger.Verbose("Publishing OnDisposedEvent.");

    EventAggregator.Publish(new OnDisposedEvent(Container, Configuration));

    Logger.Verbose("Published OnDisposedEvent.");
  }

  public virtual void ConfigureHost() {
    while (HostRegistrations.Count > 0) {
      HostRegistrations.Dequeue()();
    }
  }

  protected abstract TBuilder CreateBuilder(object args);

  protected abstract TApp CreateHost();

  protected abstract ValueTask StartHost();
}

public abstract class YggdrasilApplicationHost<THost, TBuilder, TApp> : YggdrasilHost<THost> where THost : YggdrasilApplicationHost<THost, TBuilder, TApp> {
  protected TBuilder Builder { get; set; }

  public virtual TApp Build() {
    try {
      Builder       = CreateBuilder();
      Configuration = CreateConfigurationBuilder(null).ConfigureAwait(false).GetAwaiter().GetResult().Build();
      Logger        = CreateLogger();

      Logger.Verbose("Configuring Container.");

      ConfigureContainer();

      Logger.Verbose("Configured Container.");
    } catch (HostAbortedException) { } catch (Exception e) {
      Logger.Error(e, "An error occurred setting up host and container.");

      throw;
    }

    try {
      Logger.Verbose("Creating Application.");
      
      var app = BuildApp();
      
      Logger.Verbose("Created Application.");
      Logger.Verbose("Publishing OnConfiguredEvent");

      EventAggregator.Publish(new OnConfiguredEvent(Container, Configuration));

      Logger.Verbose("Published OnConfiguredEvent");

      EventAggregator.Publish(new OnStartEvent(Registry));

      return app;
    } catch (OperationCanceledException) { } catch (Exception e) {
      Logger.Error(e, "Error running application.");

      throw;
    }

    throw new("Unknown error occurred.");
  }

  protected virtual THost ConfigureBuilder(Action<TBuilder> config) {
    Registrations.Enqueue(() => config(Builder), 1);
    return (THost)this;
  }

  protected abstract TBuilder CreateBuilder();

  protected abstract TApp BuildApp();
}