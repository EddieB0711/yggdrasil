namespace Yggdrasil.Vehicles.Api.Infrastructure.Context;

using Microsoft.EntityFrameworkCore;

using Yggdrasil.Vehicles.Domain;

public class VehiclesApiContext : DbContext {
  public VehiclesApiContext(DbContextOptions<VehiclesApiContext> options) : base(options) { }

  public DbSet<TrimEntity>? Trims { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyConfiguration(new TrimTypeConfiguration());
  }
}