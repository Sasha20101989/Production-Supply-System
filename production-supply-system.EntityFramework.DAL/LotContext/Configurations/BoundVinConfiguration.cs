using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;

public partial class BoundVinConfiguration : IEntityTypeConfiguration<BoundVin>
{
    public void Configure(EntityTypeBuilder<BoundVin> entity)
    {
        _ = entity.HasOne(d => d.VinInContainer).WithOne(p => p.BoundVin)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_tbd_Bound_VINs_tbd_VINs_In_Container");

        _ = entity.HasOne(d => d.VinNumberLocal).WithOne(p => p.BoundVin)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_tbd_Bound_VINs_tbd_VIN_Numbers_Local1");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<BoundVin> entity);
}
