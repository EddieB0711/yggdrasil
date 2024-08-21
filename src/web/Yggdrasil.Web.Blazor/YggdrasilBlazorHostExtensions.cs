namespace Yggdrasil.Web.Blazor;

using Yggdrasil.Web.Blazor.Components.Desktop;
using Yggdrasil.Web.Blazor.Components.Desktop.Models;

public static class YggdrasilBlazorHostExtensions {
  public static T UseDesktopLayout<T>(this T host, Action<BlazorConfiguration> config) where T : YggdrasilBlazorHost<T> {
    return host.ConfigureBlazor((bc, c) => {
      bc.MainLayout = typeof(DesktopLayout);
      config(bc);
    });
  }

  public static BlazorConfiguration AddMenuItem(this BlazorConfiguration config, MenuItem item) {
    config.MenuItems.Add(item);
    return config;
  }

  public static BlazorConfiguration AddMenuItems(this BlazorConfiguration config, params MenuItem[] items) {
    config.MenuItems.AddRange(items);
    return config;
  }
}