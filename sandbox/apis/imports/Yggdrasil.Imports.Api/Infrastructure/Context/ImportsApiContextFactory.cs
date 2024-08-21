namespace Yggdrasil.Imports.Api.Infrastructure.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class ImportsApiContextFactory : IDesignTimeDbContextFactory<ImportsApiContext> {
  public ImportsApiContext CreateDbContext(string[] args) {
    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
    var optionsBuilder = new DbContextOptionsBuilder<ImportsApiContext>();

    optionsBuilder.UseNpgsql(configuration["ConnectionStrings:PostgreSQL"]);

    return new(optionsBuilder.Options);
  }
}