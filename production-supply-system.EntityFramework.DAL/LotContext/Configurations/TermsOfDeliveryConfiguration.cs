using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;
public partial class TermsOfDeliveryConfiguration : IEntityTypeConfiguration<TermsOfDelivery>
{
    public void Configure(EntityTypeBuilder<TermsOfDelivery> entity)
    {
        _ = entity.HasKey(e => e.Id).HasName("PK_tbd_Delivery_Terms");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<TermsOfDelivery> entity);
}
