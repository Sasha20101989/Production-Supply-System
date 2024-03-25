using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.BomContext.Models;

namespace production_supply_system.EntityFramework.DAL.BomContext.Configurations
{
    public partial class PartConfiguration : IEntityTypeConfiguration<Part>
    {
        public void Configure(EntityTypeBuilder<Part> entity)
        {
            _ = entity.HasOne(d => d.ExtColorNavigation).WithMany(p => p.Parts).HasConstraintName("FK_tbd_Parts_tbd_ExternalColors1");

            _ = entity.HasOne(d => d.IntColorNavigation).WithMany(p => p.Parts).HasConstraintName("FK_tbd_Parts_tbd_InternalColors1");

            _ = entity.HasOne(d => d.PartType).WithMany(p => p.Parts).HasConstraintName("FK_tbd_Parts_tbd_PartsType1");

            _ = entity.HasOne(d => d.SupplierCode).WithMany(p => p.Parts).HasConstraintName("FK_tbd_Parts_tbd_Suppliers");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Part> entity);
    }
}
