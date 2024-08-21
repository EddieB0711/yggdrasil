namespace Yggdrasil;

using Microsoft.AspNetCore.Builder;

using Yggdrasil.Host.Abstractions;

public static class YggdrasilWebHostHttpsRedirectExtensions {
  public static T UseHttpsRedirect<T>(this T host) where T : IYggdrasilRunnableHost<T, WebApplicationBuilder, WebApplication> {
    return host.ConfigureHost((app, c) => app.UseHttpsRedirection());
  }
}