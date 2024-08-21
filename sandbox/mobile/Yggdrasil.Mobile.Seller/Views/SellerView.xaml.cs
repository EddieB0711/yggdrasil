namespace Yggdrasil.Mobile.Seller.Views;

using Yggdrasil.ViewModels;

public partial class SellerView : ContentPage {
  public SellerView(SellerViewModel vm) {
    InitializeComponent();

    BindingContext = vm;
  }
}