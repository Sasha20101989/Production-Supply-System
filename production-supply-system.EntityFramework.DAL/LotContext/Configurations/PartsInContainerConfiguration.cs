using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;

public partial class PartsInContainerConfiguration : IEntityTypeConfiguration<PartsInContainer>
{
    public void Configure(EntityTypeBuilder<PartsInContainer> entity)
    {
        _ = entity.HasKey(e => e.Id).HasName("PK_tbd_PartsInContainer");

        _ = entity.HasOne(d => d.ContainerInLot).WithMany(p => p.PartsInContainers)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_tbd_PartsInContainer_tbd_ContainersInLot");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<PartsInContainer> entity);
}
