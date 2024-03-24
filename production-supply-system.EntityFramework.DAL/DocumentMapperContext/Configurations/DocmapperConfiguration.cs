using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;

namespace production_supply_system.EntityFramework.DAL.DocumentMapperContext.Configurations
{
    public partial class DocmapperConfiguration : IEntityTypeConfiguration<Docmapper>
    {
        public void Configure(EntityTypeBuilder<Docmapper> entity)
        {
            _ = entity.Property(e => e.FirstDataRow).HasDefaultValue(1);
            _ = entity.Property(e => e.IsActive).HasDefaultValue(true);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Docmapper> entity);
    }
}
