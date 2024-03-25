using Microsoft.EntityFrameworkCore;
using production_supply_system.EntityFramework.DAL.MasterProcessContext.Configurations;
using production_supply_system.EntityFramework.DAL.MasterProcessContext.Models;

namespace production_supply_system.EntityFramework.DAL.Context;

public partial class MasterProcessContext : DbContext
{
    public MasterProcessContext(DbContextOptions<MasterProcessContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Process> Processes { get; set; }

    public virtual DbSet<ProcessesStep> ProcessesSteps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new ProcessConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ProcessesStepConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
