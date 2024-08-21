namespace Yggdrasil.Imports.Api.Infrastructure;

using Lamar;

using Microsoft.EntityFrameworkCore;

using Yggdrasil.Imports.Api.Infrastructure.Context;

public class ImportsApiRegistry : ServiceRegistry {
  public ImportsApiRegistry(IConfiguration configuration) {
    For<DbContext>().Use(
      _ => {
        var connectionString = configuration["ConnectionStrings:PostgreSQL"];
        var optionsBuilder = new DbContextOptionsBuilder<ImportsApiContext>();

        optionsBuilder.UseNpgsql(connectionString);

        var context = new ImportsApiContext(optionsBuilder.Options);

        context.Database.Migrate();

        return context;
      });
  }
}