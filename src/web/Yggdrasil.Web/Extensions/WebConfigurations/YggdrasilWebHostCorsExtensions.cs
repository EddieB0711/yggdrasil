namespace Yggdrasil;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Yggdrasil.Host.Abstractions;
using Yggdrasil.Web;
using Yggdrasil.Web.Extensions.Cors;

public static class YggdrasilWebHostCorsExtensions {
  public static T UseCors<T>(this T host) where T : IYggdrasilRunnableHost<T, WebApplicationBuilder, WebApplication> {
    host.ConfigureBuilder((builder, c) => builder.Services.AddCors(cors => cors.AddPolicy("CorsPolicy", b => b.Default())), MiddlewarePriority.Cors)
        .ConfigureHost((app, c) => app.UseCors("CorsPolicy"), MiddlewarePriority.Cors);

    return host;
  }
}