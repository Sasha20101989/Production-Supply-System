using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;

public partial class VinsInContainerConfiguration : IEntityTypeConfiguration<VinsInContainer>
{
    public void Configure(EntityTypeBuilder<VinsInContainer> entity)
    {
        _ = entity.HasKey(e => e.VinInContainerId).HasName("PK_tbd_Vin_In_Container");

        _ = entity.HasOne(d => d.ContainerInLot).WithMany(p => p.VinsInContainers)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_tbd_Vin_In_Container_tbd_Containers_In_Lot");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<VinsInContainer> entity);
}
