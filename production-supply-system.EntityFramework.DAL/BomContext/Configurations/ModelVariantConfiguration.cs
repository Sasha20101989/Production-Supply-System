using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.BomContext.Models;

namespace production_supply_system.EntityFramework.DAL.BomContext.Configurations
{
    public partial class ModelVariantConfiguration : IEntityTypeConfiguration<ModVar>
    {
        public void Configure(EntityTypeBuilder<ModVar> entity)
        {
            _ = entity.Property(e => e.SupplierEndItem).IsFixedLength();

            _ = entity.HasOne(d => d.EndItem).WithMany(p => p.ModelVariants)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_ModelVariants_tbd_EndItems");

            _ = entity.HasOne(d => d.ExtCol).WithMany(p => p.ModelVariants)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_ModelVariants_tbd_ExternalColors");

            _ = entity.HasOne(d => d.IntCol).WithMany(p => p.ModelVariants)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_ModelVariants_tbd_InternalColors");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ModVar> entity);
    }
}
