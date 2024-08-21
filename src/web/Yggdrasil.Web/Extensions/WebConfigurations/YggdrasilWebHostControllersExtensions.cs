namespace Yggdrasil;

using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Yggdrasil.Host.Abstractions;
using Yggdrasil.Web;
using Yggdrasil.Web.Configurations;

public static class YggdrasilWebHostControllersExtensions {
  public static T UseControllers<T>(this T host, Action<ControllerConfigurations> configure) where T : IYggdrasilRunnableHost<T, WebApplicationBuilder, WebApplication> {
    var assembly = Assembly.GetEntryAssembly();
    ControllerConfigurations configuration = new();

    host.ConfigureBuilder((b, c) => {
          configure?.Invoke(configuration);

          var builder = b.Services.AddControllers(x => configuration.MvcOptions?.Invoke(x))
                         .AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                         .AddApplicationPart(assembly);

          configuration.Builder?.Invoke(builder);
        }, MiddlewarePriority.ControllerMaps)
        .ConfigureHost((app, c) => app.MapControllers(), MiddlewarePriority.ControllerMaps);

    return host;
  }

  public static T UseControllers<T>(this T host) where T : IYggdrasilRunnableHost<T, WebApplicationBuilder, WebApplication> {
    return UseControllers(host, config => { });
  }
}