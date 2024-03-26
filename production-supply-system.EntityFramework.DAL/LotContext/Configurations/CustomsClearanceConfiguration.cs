using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;

public partial class CustomsClearanceConfiguration : IEntityTypeConfiguration<CustomsClearance>
{
    public void Configure(EntityTypeBuilder<CustomsClearance> entity)
    {
        _ = entity.HasOne(d => d.ContainerInLot).WithMany(p => p.CustomsClearances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_Customs_Clearance_tbd_Containers_In_Lot");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<CustomsClearance> entity);
}
