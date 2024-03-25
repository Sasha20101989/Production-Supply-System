using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.BomContext.Models;

namespace production_supply_system.EntityFramework.DAL.BomContext.Configurations
{
    public partial class MtcForPartsApplicationConfiguration : IEntityTypeConfiguration<MtcForPartsApplication>
    {
        public void Configure(EntityTypeBuilder<MtcForPartsApplication> entity)
        {
            _ = entity.HasOne(d => d.MtcCriteria).WithMany(p => p.MtcForPartsApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_MtcForPartsApplication_tbd_MtcCriteria");

            _ = entity.HasOne(d => d.PartApplication).WithMany(p => p.MtcForPartsApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_MtcForPartsApplication_tbd_PartsApplication");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<MtcForPartsApplication> entity);
    }
}
