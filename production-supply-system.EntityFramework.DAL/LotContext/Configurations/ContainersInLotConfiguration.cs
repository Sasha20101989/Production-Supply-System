using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;
public partial class ContainersInLotConfiguration : IEntityTypeConfiguration<ContainersInLot>
{
    public void Configure(EntityTypeBuilder<ContainersInLot> entity)
    {
        _ = entity.HasKey(e => e.Id).HasName("PK_tbd_ContainersInLot");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<ContainersInLot> entity);
}
