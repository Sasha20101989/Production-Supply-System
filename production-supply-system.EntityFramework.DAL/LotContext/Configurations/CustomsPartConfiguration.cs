using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;

public partial class CustomsPartConfiguration : IEntityTypeConfiguration<CustomsPart>
{
    public void Configure(EntityTypeBuilder<CustomsPart> entity)
    {
        entity.HasKey(e => e.PartNumberId).HasName("PK_tbd_CustomsParts");

        entity.Property(e => e.PartNumberId).ValueGeneratedNever();
        entity.Property(e => e.DateAdd).HasDefaultValueSql("(getdate())");

        entity.HasOne(d => d.PartType).WithMany(p => p.CustomsParts)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_tbd_CustomsParts_tbd_PartTypes");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<CustomsPart> entity);
}
