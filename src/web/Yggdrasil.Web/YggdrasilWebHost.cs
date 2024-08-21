namespace Yggdrasil.Web;

using Microsoft.AspNetCore.Builder;

using Yggdrasil.Host;
using Yggdrasil.Host.Abstractions;

public abstract class YggdrasilWebHost<T> : YggdrasilRunnableHost<T, WebApplicationBuilder, WebApplication> where T : YggdrasilWebHost<T> {
  protected override ValueTask DisposeHost() {
    return Host?.DisposeAsync() ?? new();
  }

  protected override WebApplicationBuilder CreateBuilder(object args) {
    var builder = WebApplication.CreateBuilder(args as string[] ?? Array.Empty<string>());

    builder.Host.UseServiceProviderFactory(new YggdrasilServiceProviderFactory(Registry, () => Container));

    return builder;
  }

  protected override WebApplication CreateHost() {
    return Builder.Build();
  }

  protected override async ValueTask StartHost() {
    await Host.RunAsync().ConfigureAwait(false);
  }
}

public class YggdrasilDefaultWebHost : YggdrasilWebHost<YggdrasilDefaultWebHost> { }

public static class YggdrasilHost {
  public static async ValueTask Run<T>(string[] args, Action<T> configureHost) where T : IYggdrasilRunnableHost<T, WebApplicationBuilder, WebApplication>, new() {
    await using var host = new T();

    configureHost(host);

    await host.Run(args).ConfigureAwait(false);
  }
}