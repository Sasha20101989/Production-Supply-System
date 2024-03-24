using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.Triggers;

using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;
using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Configurations;

namespace production_supply_system.EntityFramework.DAL.DocumentMapperContext.Context
{
    public partial class DocmapperContext : DbContextWithTriggers
    {
        public DocmapperContext()
        {
        }

        public DocmapperContext(DbContextOptions<DocmapperContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Docmapper> Docmappers { get; set; }

        public virtual DbSet<DocmapperColumn> DocmapperColumns { get; set; }

        public virtual DbSet<DocmapperContent> DocmapperContents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.ApplyConfiguration(new DocmapperConfiguration());

            _ = modelBuilder.ApplyConfiguration(new DocmapperContentConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}