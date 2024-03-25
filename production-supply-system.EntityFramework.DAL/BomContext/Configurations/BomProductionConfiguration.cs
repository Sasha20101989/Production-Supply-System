using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using production_supply_system.EntityFramework.DAL.BomContext.Models;

namespace production_supply_system.EntityFramework.DAL.BomContext.Configurations
{
    public partial class BomProductionConfiguration : IEntityTypeConfiguration<BomProduction>
    {
        public void Configure(EntityTypeBuilder<BomProduction> entity)
        {
            _ = entity.HasOne(d => d.ModelVariant).WithMany(p => p.BomProductions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_BomProduction_tbd_ModelVariants");

            _ = entity.HasOne(d => d.PartsApplication).WithMany(p => p.BomProductions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_BomProduction_tbd_PartsApplication");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<BomProduction> entity);
    }
}
