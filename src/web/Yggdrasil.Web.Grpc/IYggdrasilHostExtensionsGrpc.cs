namespace Yggdrasil;

using System.Reflection;

using Grpc.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Yggdrasil.Host.Abstractions;

public static class IYggdrasilHostExtensionsGrpc {
  public static T UseGrpc<T>(this T host, params Assembly[] serviceAssemblies) where T : IYggdrasilRunnableHost<T, WebApplicationBuilder, WebApplication> {
    return host.ConfigureServices((services, c) => services.AddGrpc())
               .ConfigureHost((app, config) => {
                 app.UseRouting();
                 app.UseGrpcWeb(new() { DefaultEnabled = true });

                 AddServices(app, serviceAssemblies);
               });
  }

  static void AddServices(WebApplication app, Assembly[] serviceAssemblies) {
    var clientBaseType = typeof(ClientBase);
    var mapGrpcService = typeof(GrpcServiceEndpointConventionBuilder).GetMethod("MapGrpcService", BindingFlags.Public | BindingFlags.Static);
    var serviceTypes = serviceAssemblies.SelectMany(a => a.GetTypes()).Where(t => t.IsSubclassOf(clientBaseType)).ToList();

    foreach (var addService in serviceTypes.Select(t => mapGrpcService.MakeGenericMethod(t))) {
      addService.Invoke(null, new object[] { app });
    }
  }
}