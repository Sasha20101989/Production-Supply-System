using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using production_supply_system.EntityFramework.DAL.LotContext.Models;


namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;

public partial class PartsInInvoiceConfiguration : IEntityTypeConfiguration<PartsInInvoice>
{
    public void Configure(EntityTypeBuilder<PartsInInvoice> entity)
    {
        _ = entity.HasKey(e => e.PartInInvoiceId).HasName("PK_tbd_PartsInInvoice");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<PartsInInvoice> entity);
}
