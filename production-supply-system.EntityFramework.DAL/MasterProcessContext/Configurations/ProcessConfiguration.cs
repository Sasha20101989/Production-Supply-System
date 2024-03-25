using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using production_supply_system.EntityFramework.DAL.MasterProcessContext.Models;

namespace production_supply_system.EntityFramework.DAL.MasterProcessContext.Configurations;

public partial class ProcessConfiguration : IEntityTypeConfiguration<Process>
{
    public void Configure(EntityTypeBuilder<Process> entity)
    {
        _ = entity.HasKey(e => e.Id).HasName("PK_Processes");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Process> entity);
}
