namespace Yggdrasil.Imports.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

using Yggdrasil.Dispatch.Abstractions;
using Yggdrasil.Imports.Api.Controllers.Requests;
using Yggdrasil.Imports.Domain.Dispatchables;
using Yggdrasil.Imports.Domain.Entities;

[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
public class CrsImportsController : ControllerBase {
  readonly IYggdrasilDispatch dispatch_;

  public CrsImportsController(IYggdrasilDispatch dispatch) {
    dispatch_ = dispatch;
  }

  [HttpGet("imports/crs/{filename}")]
  [ProducesResponseType(typeof(CrsImportEntity), StatusCodes.Status200OK)]
  public IActionResult GetCrsImport([FromRoute] string filename) {
    var result = dispatch_.Dispatch(new GetCrsImport(filename));

    return result.Match<IActionResult>(Ok, BadRequest);
  }

  [HttpPost("imports/crs")]
  [ProducesResponseType(typeof(CrsImportEntity), StatusCodes.Status200OK)]
  public IActionResult RunCrsImport([FromBody] RunCrsImportRequest request) {
    var result = dispatch_.Dispatch(new ImportCrsFile(request.Filename));

    return result.Match<IActionResult>(Ok, BadRequest);
  }
}