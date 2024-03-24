using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.LotContext.Models;


namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;
public partial class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> entity)
    {
        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Case> entity);
}
