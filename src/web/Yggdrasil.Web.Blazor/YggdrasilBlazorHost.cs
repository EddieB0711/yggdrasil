namespace Yggdrasil.Web.Blazor;

using System.Reflection;

using Lamar;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;

using Yggdrasil.Host;
using Yggdrasil.Host.Abstractions;
using Yggdrasil.Web.Blazor.Components;
using Yggdrasil.Web.Blazor.Components.Desktop.Models;

public class BlazorConfiguration {
  public List<Assembly> Assemblies { get; } = new();
  public List<MenuItem> MenuItems { get; } = new();
  public Type MainLayout { get; set; }
}

public abstract class YggdrasilBlazorHost<T> : YggdrasilRunnableHost<T, WebAssemblyHostBuilder, WebAssemblyHost> where T : YggdrasilBlazorHost<T> {
  protected BlazorConfiguration BlazorConfiguration { get; } = new();

  public T UseAssemblies(params Assembly[] assemblies) {
    BlazorConfiguration.Assemblies.AddRange(assemblies);
    return (T)this;
  }

  public T UseComponent<TComponent>(string selector) where TComponent : IComponent {
    Registrations.Enqueue(() => Builder.RootComponents.Add<TComponent>(selector), 1);
    return (T)this;
  }

  public T ConfigureBlazor(Action<BlazorConfiguration, IConfiguration> config) {
    Registrations.Enqueue(() => config(BlazorConfiguration, Configuration), 1);
    return (T)this;
  }

  protected override void ConfigureContainer() {
    Registry.For<BlazorConfiguration>().Use(BlazorConfiguration);

    Registry.For<HttpClient>().Use(_ => new() { BaseAddress = new(Builder.HostEnvironment.BaseAddress) }).Scoped();

    base.ConfigureContainer();
  }

  protected override ValueTask DisposeHost() {
    return Host?.DisposeAsync() ?? new ValueTask();
  }

  protected override WebAssemblyHostBuilder CreateBuilder(object args) {
    var builder = WebAssemblyHostBuilder.CreateDefault(args as string[]);

    builder.ConfigureContainer(new YggdrasilServiceProviderFactory(Registry, () => Container));

    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    return builder;
  }

  protected override WebAssemblyHost CreateHost() {
    return Builder.Build();
  }

  protected override async ValueTask StartHost() {
    await Host.RunAsync().ConfigureAwait(false);
  }
}

public class YggdrasilDefaultBlazorHost : YggdrasilBlazorHost<YggdrasilDefaultBlazorHost> { }

public static class YggdrasilHost {
  public static async ValueTask Run<T>(string[] args, Action<T> configureHost) where T : IYggdrasilRunnableHost<T, WebAssemblyHostBuilder, WebAssemblyHost>, new() {
    await using var host = new T();

    configureHost(host);

    await host.Run(args).ConfigureAwait(false);
  }
}