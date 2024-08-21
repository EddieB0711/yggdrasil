namespace Yggdrasil.Vehicles.Api.Infrastructure.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Yggdrasil.Vehicles.Domain;

public class TrimTypeConfiguration : IEntityTypeConfiguration<TrimEntity> {
  public void Configure(EntityTypeBuilder<TrimEntity> builder) {
    builder.ToTable("CRS_Trim");
  }
}