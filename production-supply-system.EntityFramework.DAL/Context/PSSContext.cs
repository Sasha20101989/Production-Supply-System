using Microsoft.EntityFrameworkCore;
using production_supply_system.EntityFramework.DAL.Models.dboSchema;
using production_supply_system.EntityFramework.DAL.Models.UsersSchema;
using production_supply_system.EntityFramework.DAL.Models.InboundSchema;

namespace production_supply_system.EntityFramework.DAL.Context;

public partial class PSSContext : DbContext
{
    public PSSContext(DbContextOptions<PSSContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<TermsOfContainerUse> TermsOfContainerUses { get; set; }

    public virtual DbSet<ImoCargo> ImoCargos { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new Models.Configurations.SectionConfiguration());
        _ = modelBuilder.ApplyConfiguration(new Models.Configurations.TermsOfContainerUseConfiguration());
        _ = modelBuilder.ApplyConfiguration(new Models.Configurations.UserConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
