using Microsoft.EntityFrameworkCore;

using production_supply_system.EntityFramework.DAL.BomContext.Models;

namespace production_supply_system.EntityFramework.DAL.BomContext;

public partial class BomContext : DbContext
{
    public BomContext()
    {
    }

    public BomContext(DbContextOptions<BomContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BomProduction> BomProductions { get; set; }

    public virtual DbSet<EndItem> EndItems { get; set; }

    public virtual DbSet<ExternalColor> ExternalColors { get; set; }

    public virtual DbSet<InternalColor> InternalColors { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<ModVar> ModelVariants { get; set; }

    public virtual DbSet<MtcCriterion> MtcCriteria { get; set; }

    public virtual DbSet<MtcForEndItem> MtcForEndItems { get; set; }

    public virtual DbSet<MtcForPartsApplication> MtcForPartsApplications { get; set; }

    public virtual DbSet<MtcGroup> MtcGroups { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<PartsApplication> PartsApplications { get; set; }

    public virtual DbSet<PartsType> PartsTypes { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new Configurations.BomProductionConfiguration());
        _ = modelBuilder.ApplyConfiguration(new Configurations.EndItemConfiguration());
        _ = modelBuilder.ApplyConfiguration(new Configurations.ModelVariantConfiguration());
        _ = modelBuilder.ApplyConfiguration(new Configurations.MtcCriterionConfiguration());
        _ = modelBuilder.ApplyConfiguration(new Configurations.MtcForEndItemConfiguration());
        _ = modelBuilder.ApplyConfiguration(new Configurations.MtcForPartsApplicationConfiguration());
        _ = modelBuilder.ApplyConfiguration(new Configurations.PartConfiguration());
        _ = modelBuilder.ApplyConfiguration(new Configurations.PartsApplicationConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
