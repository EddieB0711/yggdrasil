using Yggdrasil;
using Yggdrasil.Web.Blazor;

using Index = Yggdrasil.Sandbox.FrontEnd.Pages.Index;

await YggdrasilHost.Run<YggdrasilDefaultBlazorHost>(args, ConfigureHost);

static void ConfigureHost(YggdrasilDefaultBlazorHost host) =>
  host.UseSerilog()
      .UseAssemblies(typeof(Index).Assembly)
      .UseDesktopLayout(config => config.AddMenuItem(new() { Selected = true, Title  = "Home", Icon    = "home-outline", Path      = "/" })
                                        .AddMenuItem(new() { Selected = false, Title = "Imports", Icon = "download-outline", Path  = "/imports" })
                                        .AddMenuItem(new() { Selected = false, Title = "Jobs", Icon    = "briefcase-outline", Path = "#" }));