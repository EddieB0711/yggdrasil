namespace Yggdrasil.Mobile.Host;

using Microsoft.Maui.LifecycleEvents;

using Yggdrasil.Host;

public class YggdrasilMobileHost<THost> : YggdrasilApplicationHost<THost, MauiAppBuilder, MauiApp> where THost : YggdrasilMobileHost<THost> {
  public THost UseApp<T>() where T : Application {
    Registrations.Enqueue(() => Builder.UseMauiApp<T>(), 1);
    return (THost)this;
  }

  protected override ValueTask DisposeHost() {
    return new();
  }

  protected override MauiAppBuilder CreateBuilder() {
    var builder = MauiApp.CreateBuilder();

    builder.ConfigureLifecycleEvents(
             events => {
#if ANDROID
               events.AddAndroid(android => android.OnDestroy(activity => Dispose()));
#endif
             })
           .ConfigureFonts(fonts => {
             fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
             fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
             fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");
             fonts.AddFont("Poppins-SemiBold.ttf", "PoppinsSimibold");
           })
           .ConfigureContainer(new YggdrasilServiceProviderFactory(Registry, () => Container));

    return builder;
  }

  protected override MauiApp BuildApp() {
    return Builder.Build();
  }
}

public class YggdrasilDefaultMobileHost : YggdrasilMobileHost<YggdrasilDefaultMobileHost> { }