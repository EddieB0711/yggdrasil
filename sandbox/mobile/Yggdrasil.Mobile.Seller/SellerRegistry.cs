namespace Yggdrasil;

using CommunityToolkit.Mvvm.ComponentModel;

using Lamar;

public class SellerRegistry : ServiceRegistry {
  public SellerRegistry() {
    Scan(s => {
      s.TheCallingAssembly();
      s.WithDefaultConventions();

      s.AddAllTypesOf(typeof(ObservableObject));
    });
  }
}