namespace Yggdrasil.Vehicles.Domain;

public class TrimEntity {
  public string ProductTypeCode { get; set; }
  public int MakeId { get; set; }
  public string MakeName { get; set; }
  public int ModelId { get; set; }
  public short ModelYear { get; set; }
  public string ModelName { get; set; }
  public int TrimId { get; set; }
  public string TrimName { get; set; }
  public string TrimDisplayName { get; set; }
  public decimal? TrimMSRP { get; set; }
  public byte[] TrimPhotoFileHash { get; set; }
  public string TrimPhotoFilePath { get; set; }
}