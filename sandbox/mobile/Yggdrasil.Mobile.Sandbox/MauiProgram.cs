namespace Yggdrasil.Mobile.Sandbox;

using Yggdrasil.Mobile.Host;

public static class MauiProgram {
  public static MauiApp CreateMauiApp() {
    var host = new YggdrasilDefaultMobileHost();

    host.UseApp<App>()
        .UseSerilog()
        .UseDispatch()
        .UseSeller();

    return host.Build();
  }
}