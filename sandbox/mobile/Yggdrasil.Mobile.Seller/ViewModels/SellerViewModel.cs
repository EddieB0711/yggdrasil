namespace Yggdrasil.ViewModels;

using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Yggdrasil.Mobile.Seller.Models;

public partial class SellerViewModel : ObservableObject {
  [ObservableProperty] ObservableCollection<Package> _packages = new();

  public SellerViewModel() {
    Packages.Add(new() {
      Id = 0,
      Name = "Product 1",
      Quantity = 100,
      Strain = new() {Name = "Biscotti"},
      UnitOfMeasure = "Pounds",
      Listings = new(),
    });

    Packages.Add(new() {
      Id = 0,
      Name = "Product 2",
      Quantity = 100,
      Strain = new() {Name = "Haze"},
      UnitOfMeasure = "Pounds",
      Listings = new(),
    });
  }

  [RelayCommand]
  void NavigateToPackageDetail() { }
}