using Yggdrasil;
using Yggdrasil.Imports.Api.Infrastructure;
using Yggdrasil.Web;

await YggdrasilHost.Run<YggdrasilDefaultWebHost>(args, ConfigureHost);

static void ConfigureHost(YggdrasilDefaultWebHost host) =>
  host.UseControllers()
      .UseCors()
      .UseSwagger("Yggdrasil Imports API", "v1")
      .UseSerilog()
      .UseHttpsRedirect()
      .UseImportsInfrastructure()
      .ConfigureServices(c => new ImportsApiRegistry(c));