using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;

public partial class TypesOfPackingConfiguration : IEntityTypeConfiguration<TypesOfPacking>
{
    public void Configure(EntityTypeBuilder<TypesOfPacking> entity)
    {
        _ = entity.HasKey(e => e.Id).HasName("PK_tbd_PackingTypes");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<TypesOfPacking> entity);
}
