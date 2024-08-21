namespace Yggdrasil.Mobile.Seller.Models; 

public class Package {
  public int Id { get; set; }

  public string Name { get; set; }

  public string UnitOfMeasure { get; set; }

  public double Quantity { get; set; }

  public Strain Strain { get; set; }

  public List<Listing> Listings { get; set; } = new();
}