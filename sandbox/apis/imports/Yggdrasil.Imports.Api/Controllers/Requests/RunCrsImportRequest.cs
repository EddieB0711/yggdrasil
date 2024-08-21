namespace Yggdrasil.Imports.Api.Controllers.Requests;

using System.ComponentModel.DataAnnotations;

public class RunCrsImportRequest {
  [Required] public string Filename { get; set; }
}