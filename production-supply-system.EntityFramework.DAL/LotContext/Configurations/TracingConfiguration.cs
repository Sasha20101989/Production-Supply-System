using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations
{
    public partial class TracingConfiguration : IEntityTypeConfiguration<Tracing>
    {
        public void Configure(EntityTypeBuilder<Tracing> entity)
        {
            _ = entity.HasOne(d => d.ContainerInLot).WithMany(p => p.Tracings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_Tracing_tbd_Containers_In_Lot");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Tracing> entity);
    }
}
