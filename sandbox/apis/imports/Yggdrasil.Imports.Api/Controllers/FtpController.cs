namespace Yggdrasil.Imports.Api.Controllers;

using System.Net;

using Microsoft.AspNetCore.Mvc;

using Yggdrasil.Imports.Api.Controllers.Requests;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
public class FtpController : ControllerBase {
  [HttpPost("ftp/files")]
  [ProducesResponseType(typeof(string[]), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetFtpFiles([FromBody] GetFtpFilesRequest request) {
    var ftpRequest = (FtpWebRequest)WebRequest.Create(request.Host);

    ftpRequest.Method      = WebRequestMethods.Ftp.ListDirectory;
    ftpRequest.Credentials = new NetworkCredential(request.User, request.Pass);

    using var response = (FtpWebResponse)ftpRequest.GetResponse();

    await using var stream = response.GetResponseStream();

    using var reader = new StreamReader(stream);

    var files = new List<string>();
    var line = await reader.ReadLineAsync();

    while (!string.IsNullOrWhiteSpace(line)) {
      files.Add(line);
      line = await reader.ReadLineAsync();
    }

    return Ok(files);
  }
}