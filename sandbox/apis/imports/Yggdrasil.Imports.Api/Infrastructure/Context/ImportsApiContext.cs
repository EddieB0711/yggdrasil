namespace Yggdrasil.Imports.Api.Infrastructure.Context;

using Microsoft.EntityFrameworkCore;

using Yggdrasil.Imports.Domain.Entities;

public class ImportsApiContext : DbContext {
  public ImportsApiContext(DbContextOptions options) : base(options) { }

  public DbSet<CrsImportEntity>? CrsImports { get; set; }
}