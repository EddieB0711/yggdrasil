namespace Yggdrasil;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Yggdrasil.Host.Abstractions;
using Yggdrasil.Web;
using Yggdrasil.Web.Configurations;

public static class YggdrasilWebHostSignalRExtensions {
  public static T UseSignalR<T>(this T configurations, Action<SignalRConfigurations> configure)
    where T : IYggdrasilRunnableHost<T, WebApplicationBuilder, WebApplication> {
    SignalRConfigurations configuration = new();

    configurations.ConfigureBuilder(
                    (b, c) => {
                      configure(configuration);

                      var builder = b.Services.AddSignalR(opts => configuration.HubOptions?.Invoke(opts));
                      configuration.ServerBuilder?.Invoke(builder);
                    })
                  .ConfigureHost((app, c) => app.UseRouting(), MiddlewarePriority.Routing);

    return configurations;
  }
}