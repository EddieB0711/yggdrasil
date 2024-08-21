namespace Yggdrasil.Imports.Domain.Entities;

public enum ImportStatus {
  Unknown,
  Imported,
}

public class CrsImportEntity {
  public string Filename { get; set; }
  public Guid Id { get; set; }
  public ImportStatus ImportStatus { get; set; }
  public DateTime? ImportDate { get; set; }
}