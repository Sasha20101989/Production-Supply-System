using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;

public partial class TypesOfPartConfiguration : IEntityTypeConfiguration<TypesOfPart>
{
    public void Configure(EntityTypeBuilder<TypesOfPart> entity)
    {
        _ = entity.HasKey(e => e.Id).HasName("PK_tbd_PartTypes");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<TypesOfPart> entity);
}
