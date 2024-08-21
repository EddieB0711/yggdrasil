using Yggdrasil;
using Yggdrasil.Vehicles.Api.Infrastructure.Registries;
using Yggdrasil.Web;

await YggdrasilHost.Run<YggdrasilDefaultWebHost>(args, ConfigureHost);

static void ConfigureHost(YggdrasilDefaultWebHost host) =>
  host.UseControllers()
      .UseCors()
      .UseHttpsRedirect()
      .UseSwagger("Yggdrasil Vehicles API", "v1")
      .UseSerilog()
      .UseDispatch()
      .UseGrpc(typeof(Vehicles.VehiclesClient).Assembly)
      .ConfigureServices(new VehiclesApiRegistry());