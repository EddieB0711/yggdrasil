namespace Yggdrasil.Vehicles.Api.Infrastructure.Registries;

using Lamar;

using Microsoft.EntityFrameworkCore;

using Yggdrasil.Vehicles.Api.Infrastructure.Context;

public class VehiclesApiRegistry : ServiceRegistry {
  public VehiclesApiRegistry() {
    For<DbContext>().Use(
      s => {
        var config = s.GetInstance<IConfiguration>();
        var builder = new DbContextOptionsBuilder<VehiclesApiContext>();

        builder.UseSqlServer(config["ConnectionStrings:SqlServer"]);

        return new VehiclesApiContext(builder.Options);
      });
  }
}