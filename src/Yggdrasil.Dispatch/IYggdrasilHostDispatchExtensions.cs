namespace Yggdrasil;

using Lamar;

using Yggdrasil.Dispatch;
using Yggdrasil.Dispatch.Abstractions;
using Yggdrasil.Host.Abstractions;

public static class IYggdrasilHostDispatchExtensions {
  public static T UseDispatch<T>(this T host) where T : IYggdrasilHost<T> {
    return host.ConfigureServices((r, c) => r.For<IYggdrasilDispatch>().Use<YggdrasilDispatch>().Transient());
  }
}