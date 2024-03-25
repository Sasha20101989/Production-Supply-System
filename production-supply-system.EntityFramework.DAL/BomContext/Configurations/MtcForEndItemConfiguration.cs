using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.BomContext.Models;

namespace production_supply_system.EntityFramework.DAL.BomContext.Configurations
{
    public partial class MtcForEndItemConfiguration : IEntityTypeConfiguration<MtcForEndItem>
    {
        public void Configure(EntityTypeBuilder<MtcForEndItem> entity)
        {
            _ = entity.HasOne(d => d.EndItem).WithMany(p => p.MtcForEndItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_MtcForEndItems_tbd_EndItems");

            _ = entity.HasOne(d => d.MtcCriteria).WithMany(p => p.MtcForEndItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_MtcForEndItems_tbd_MtcCriteria");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<MtcForEndItem> entity);
    }
}
