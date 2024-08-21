namespace Yggdrasil;

using Grpc.Net.Client;

using Lamar;

using Microsoft.Extensions.Configuration;

using Yggdrasil.Dispatch.Abstractions;

/// <summary>
///   The object containing all registrations from the Infrastructure project.
/// </summary>
public class ImportsInfrastructureRegistry : ServiceRegistry {
  /// <summary>
  ///   The constructor where dependencies are registered.
  /// </summary>
  public ImportsInfrastructureRegistry() {
    Scan(
      s => {
        s.TheCallingAssembly();
        s.WithDefaultConventions();

        s.ConnectImplementationsToTypesClosing(typeof(IYggdrasilDispatchableHandler<,>));
      });

    For<GrpcChannel>()
      .Use(c => CreateChannel(c, "VehiclesService"))
      .Scoped()
      .Named("VehiclesChannel");

    For<Vehicles.VehiclesClient>()
      .Use(c => new(c.GetInstance<GrpcChannel>("VehiclesChannel")))
      .Scoped();
  }

  /// <summary>
  ///   Creates a Grpc Channel for the specified name.
  /// </summary>
  /// <param name="context">The DI container (used to resolve an instance of IConfiguration).</param>
  /// <param name="name">The name of the key configuration to get the address of the service.</param>
  /// <returns>
  ///   <see cref="Grpc.Net.Client.GrpcService" />
  /// </returns>
  static GrpcChannel CreateChannel(IServiceContext context, string name) {
    var config = context.GetInstance<IConfiguration>();
    var channel = GrpcChannel.ForAddress(config[name]);

    return channel;
  }
}