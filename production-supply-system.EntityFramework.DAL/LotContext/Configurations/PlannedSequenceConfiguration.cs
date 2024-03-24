using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;

public partial class PlannedSequenceConfiguration : IEntityTypeConfiguration<PlannedSequence>
{
    public void Configure(EntityTypeBuilder<PlannedSequence> entity)
    {
        _ = entity.Property(e => e.IsSuspicious).HasDefaultValue(0);
        _ = entity.Property(e => e.IsUnyelding).HasDefaultValue(0);
        _ = entity.Property(e => e.StatusForSfsUploadId).HasDefaultValue(1);

        _ = entity.HasOne(d => d.StatusForSfsUpload).WithMany(p => p.PlannedSequences).HasConstraintName("FK_tbd_PlannedSequence_tbd_StatusesForSFSUpload");

        _ = entity.HasOne(d => d.VinInContainer).WithOne(p => p.PlannedSequence)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_tbd_Planned_Sequence_tbd_VINs_In_Container");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<PlannedSequence> entity);
}
