using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;
public partial class TypesOfTransportConfiguration : IEntityTypeConfiguration<TypesOfTransport>
{
    public void Configure(EntityTypeBuilder<TypesOfTransport> entity)
    {
        _ = entity.HasKey(e => e.TransportTypeId).HasName("PK_tbd_Type_Of_Transportation");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<TypesOfTransport> entity);
}
