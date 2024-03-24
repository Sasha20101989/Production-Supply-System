using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations
{
    public partial class StatusesForSfsUploadConfiguration : IEntityTypeConfiguration<StatusesForSfsUpload>
    {
        public void Configure(EntityTypeBuilder<StatusesForSfsUpload> entity)
        {
            _ = entity.HasKey(e => e.StatusForSfsUploadId).HasName("PK_tbd_StatusesForSFSUpload");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<StatusesForSfsUpload> entity);
    }
}
