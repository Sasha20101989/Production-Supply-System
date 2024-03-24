using Microsoft.EntityFrameworkCore;

using production_supply_system.EntityFramework.DAL.LotContext.Configurations;
using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.LotContext;

public partial class LotContext : DbContext
{
    public LotContext(DbContextOptions<LotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BoundVin> BoundVins { get; set; }

    public virtual DbSet<Carrier> Carriers { get; set; }

    public virtual DbSet<Case> Cases { get; set; }

    public virtual DbSet<ContainersInLot> ContainersInLots { get; set; }

    public virtual DbSet<CustomsClearance> CustomsClearances { get; set; }

    public virtual DbSet<CustomsPart> CustomsParts { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Lot> Lots { get; set; }

    public virtual DbSet<PartsInContainer> PartsInContainers { get; set; }

    public virtual DbSet<PartsInInvoice> PartsInInvoices { get; set; }

    public virtual DbSet<PlannedSequence> PlannedSequences { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    public virtual DbSet<StatusesForSfsUpload> StatusesForSfsUploads { get; set; }

    public virtual DbSet<TermsOfDelivery> TermsOfDeliveries { get; set; }

    public virtual DbSet<Tracing> Tracings { get; set; }

    public virtual DbSet<TypesOfContainer> TypesOfContainers { get; set; }

    public virtual DbSet<TypesOfLocation> TypesOfLocations { get; set; }

    public virtual DbSet<TypesOfOrder> TypesOfOrders { get; set; }

    public virtual DbSet<TypesOfPacking> TypesOfPackings { get; set; }

    public virtual DbSet<TypesOfPart> TypesOfParts { get; set; }

    public virtual DbSet<TypesOfTransport> TypesOfTransports { get; set; }

    public virtual DbSet<VinNumbersLocal> VinNumbersLocals { get; set; }

    public virtual DbSet<VinsInContainer> VinsInContainers { get; set; }

    public virtual DbSet<Transport> Transports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new LotConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ContainersInLotConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TermsOfDeliveryConfiguration());
        _ = modelBuilder.ApplyConfiguration(new LocationConfiguration());
        _ = modelBuilder.ApplyConfiguration(new ShipperConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PurchaseOrderConfiguration());
        _ = modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TypesOfTransportConfiguration());
        _ = modelBuilder.ApplyConfiguration(new CustomsClearanceConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PartsInContainerConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TracingConfiguration());
        _ = modelBuilder.ApplyConfiguration(new CustomsPartConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TypesOfPartConfiguration());
        _ = modelBuilder.ApplyConfiguration(new VinsInContainerConfiguration());
        _ = modelBuilder.ApplyConfiguration(new CaseConfiguration());
        _ = modelBuilder.ApplyConfiguration(new BoundVinConfiguration());
        _ = modelBuilder.ApplyConfiguration(new TypesOfPackingConfiguration());
        _ = modelBuilder.ApplyConfiguration(new StatusesForSfsUploadConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PlannedSequenceConfiguration());
        _ = modelBuilder.ApplyConfiguration(new PartsInInvoiceConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
