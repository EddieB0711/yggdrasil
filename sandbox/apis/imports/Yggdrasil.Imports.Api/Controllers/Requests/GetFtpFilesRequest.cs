namespace Yggdrasil.Imports.Api.Controllers.Requests;

using System.ComponentModel.DataAnnotations;

public class GetFtpFilesRequest {
  [Required] public string Host { get; set; }
  [Required] public string User { get; set; }
  [Required] public string Pass { get; set; }
}