using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using production_supply_system.EntityFramework.DAL.LotContext.Models;


namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;

public partial class PartsInInvoiceConfiguration : IEntityTypeConfiguration<PartsInInvoice>
{
    public void Configure(EntityTypeBuilder<PartsInInvoice> entity)
    {
        _ = entity.HasKey(e => e.PartInInvoiceId).HasName("PK_tbd_PartsInInvoice");

        _ = entity.HasOne(d => d.Invoice).WithMany(p => p.PartsInInvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_PartsInInvoice_tbd_Invoices1");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<PartsInInvoice> entity);
}
