namespace Yggdrasil.Host;

using Lamar;

using Microsoft.Extensions.DependencyInjection;

public class YggdrasilServiceProvider : IServiceProvider, ISupportRequiredService, IServiceProviderIsService {
  readonly IContainer _container;

  public YggdrasilServiceProvider(IContainer container) {
    _container = container;
  }

  public object? GetService(Type serviceType) {
    return _container.TryGetInstance(serviceType);
  }

  public bool IsService(Type serviceType) {
    return _container.TryGetInstance(serviceType) != null;
  }

  public object GetRequiredService(Type serviceType) {
    return _container.TryGetInstance(serviceType);
  }
}

public class YggdrasilServiceProviderFactory : IServiceProviderFactory<ServiceRegistry> {
  readonly Func<IContainer> _containerFactory;
  readonly ServiceRegistry _serviceRegistry;

  public YggdrasilServiceProviderFactory(ServiceRegistry registry, Func<IContainer> containerFactory) {
    _serviceRegistry  = registry;
    _containerFactory = containerFactory;
  }

  public ServiceRegistry CreateBuilder(IServiceCollection services) {
    _serviceRegistry.AddRange(services);

    _serviceRegistry.For<IServiceProvider>().Use<YggdrasilServiceProvider>();
    _serviceRegistry.For<IServiceProviderIsService>().Use<YggdrasilServiceProvider>();

    return _serviceRegistry;
  }

  public IServiceProvider CreateServiceProvider(ServiceRegistry containerBuilder) {
    return new YggdrasilServiceProvider(_containerFactory());
  }
}