namespace Yggdrasil;

using Yggdrasil.Mobile.Host;

public static class SellerExtensions {
  public static T UseSeller<T>(this T host) where T : YggdrasilMobileHost<T> {
    return host.ConfigureServices(new SellerRegistry());
  }
}