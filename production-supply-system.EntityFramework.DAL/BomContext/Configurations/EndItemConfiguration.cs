using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.BomContext.Models;

namespace production_supply_system.EntityFramework.DAL.BomContext.Configurations
{
    public partial class EndItemConfiguration : IEntityTypeConfiguration<EndItem>
    {
        public void Configure(EntityTypeBuilder<EndItem> entity)
        {
            _ = entity.HasOne(d => d.Model).WithMany(p => p.EndItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_EndItems_tbd_Models");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<EndItem> entity);
    }
}
