using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using production_supply_system.EntityFramework.DAL.Models.InboundSchema;

namespace production_supply_system.EntityFramework.DAL.Models.Configurations
{
    public partial class TermsOfContainerUseConfiguration : IEntityTypeConfiguration<TermsOfContainerUse>
    {
        public void Configure(EntityTypeBuilder<TermsOfContainerUse> entity)
        {
            _ = entity.HasKey(e => e.Id).HasName("PK_tbd_TracingEventStaticData");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TermsOfContainerUse> entity);
    }
}
