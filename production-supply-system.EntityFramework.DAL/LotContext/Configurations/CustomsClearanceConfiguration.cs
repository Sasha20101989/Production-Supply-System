using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext.Configurations;

public partial class CustomsClearanceConfiguration : IEntityTypeConfiguration<CustomsClearance>
{
    public void Configure(EntityTypeBuilder<CustomsClearance> entity)
    {
        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<CustomsClearance> entity);
}
