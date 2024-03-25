using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.BomContext.Models;

namespace production_supply_system.EntityFramework.DAL.BomContext.Configurations
{
    public partial class MtcCriterionConfiguration : IEntityTypeConfiguration<MtcCriterion>
    {
        public void Configure(EntityTypeBuilder<MtcCriterion> entity)
        {
            _ = entity.Property(e => e.CriteriaDescription).IsFixedLength();

            _ = entity.HasOne(d => d.MtcGroup).WithMany(p => p.MtcCriteria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_MtcCriteria_tbd_MtcGroups");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<MtcCriterion> entity);
    }
}
