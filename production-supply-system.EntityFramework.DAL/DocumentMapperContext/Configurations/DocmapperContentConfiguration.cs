using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;

namespace production_supply_system.EntityFramework.DAL.DocumentMapperContext.Configurations
{
    public partial class DocmapperContentConfiguration : IEntityTypeConfiguration<DocmapperContent>
    {
        public void Configure(EntityTypeBuilder<DocmapperContent> entity)
        {
            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<DocmapperContent> entity);
    }
}
