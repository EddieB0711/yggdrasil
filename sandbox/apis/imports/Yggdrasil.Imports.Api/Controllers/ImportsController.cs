namespace Yggdrasil.Imports.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
public class ImportsController : ControllerBase {
  [HttpGet("imports")]
  public IActionResult GetImports() {
    return Ok(new[] { "CRS Import", "Third Party Import", "Dealerspike Import", "SpinCar Import", "Talon Import", "Lightspeed Import" });
  }

  [HttpPost("import/run/{name}")]
  public async Task<IActionResult> RunImport([FromRoute] string name) {
    return Ok();
  }
}