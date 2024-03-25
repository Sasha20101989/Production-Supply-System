using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;

public partial class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> entity)
    {
        _ = entity.HasKey(e => e.Id).HasName("PK_tbd_PurchaseOrders");

        _ = entity.HasOne(e => e.Shipper).WithMany(p => p.PurchaseOrders)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_tbd_Purchase_Orders_tbd_Shippers");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<PurchaseOrder> entity);
}
