namespace Yggdrasil.Web.Configurations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

public class ControllerConfigurations {
  public Action<MvcOptions> MvcOptions { get; set; }
  public Action<IMvcBuilder> Builder { get; set; }
}