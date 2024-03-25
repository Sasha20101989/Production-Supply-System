using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;

public partial class CustomsPartConfiguration : IEntityTypeConfiguration<CustomsPart>
{
    public void Configure(EntityTypeBuilder<CustomsPart> entity)
    {
        _ = entity.HasKey(e => e.PartNumberId).HasName("PK_tbd_CustomsParts");

        _ = entity.Property(e => e.PartNumberId).ValueGeneratedNever();

        _ = entity.Property(e => e.DateAdd).HasDefaultValueSql("(getdate())");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<CustomsPart> entity);
}
