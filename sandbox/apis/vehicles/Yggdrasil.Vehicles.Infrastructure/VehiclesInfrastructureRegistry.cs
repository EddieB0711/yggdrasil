namespace Yggdrasil.Vehicles.Infrastructure;

using Lamar;

using Yggdrasil.Dispatch.Abstractions;
using Yggdrasil.Vehicles.Infrastructure.Handlers.Dispatchables;

public class VehiclesInfrastructureRegistry : ServiceRegistry {
  public VehiclesInfrastructureRegistry() {
    Scan(
      s => {
        s.TheCallingAssembly();
        s.WithDefaultConventions();

        s.ConnectImplementationsToTypesClosing(typeof(IYggdrasilDispatchableHandler<,>));
        s.ConnectImplementationsToTypesClosing(typeof(IYggdrasilAsyncDispatchableHandler<,>));
        s.ConnectImplementationsToTypesClosing(typeof(IYggdrasilStreambleDispatchableHandler<,>));
      });

#if DEBUG
    For(typeof(IYggdrasilDispatchableHandler<,>))
      .DecorateAllWith(typeof(TimeFunctionHandler<,>));
    For(typeof(IYggdrasilAsyncDispatchableHandler<,>))
      .DecorateAllWith(typeof(TimeAsyncFunctionHandler<,>));
    For(typeof(IYggdrasilStreambleDispatchableHandler<,>))
      .DecorateAllWith(typeof(TimeStreamableFunctionHandler<,>));
#endif
  }
}