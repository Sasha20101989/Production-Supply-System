using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using production_supply_system.EntityFramework.DAL.BomContext.Models;

namespace production_supply_system.EntityFramework.DAL.BomContext.Configurations
{
    public partial class PartsApplicationConfiguration : IEntityTypeConfiguration<PartsApplication>
    {
        public void Configure(EntityTypeBuilder<PartsApplication> entity)
        {
            _ = entity.HasKey(e => e.Id).HasName("PK_tbd_PartsApplication_1");

            _ = entity.HasOne(d => d.Model).WithMany(p => p.PartsApplications).HasConstraintName("FK_tbd_PartsApplication_tbd_Models1");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PartsApplication> entity);
    }
}
