﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using production_supply_system.EntityFramework.DAL.TestContext.Configurations;
using production_supply_system.EntityFramework.DAL.TestContext.CustomsSchema;
using production_supply_system.EntityFramework.DAL.TestContext.dboSchema;
using production_supply_system.EntityFramework.DAL.TestContext.DocmapperSchema;
using production_supply_system.EntityFramework.DAL.TestContext.InboundSchema;
using production_supply_system.EntityFramework.DAL.TestContext.MasterSchema;
using production_supply_system.EntityFramework.DAL.TestContext.PartscontrolSchema;
using production_supply_system.EntityFramework.DAL.TestContext.PlanningSchema;
using production_supply_system.EntityFramework.DAL.TestContext.UsersSchema;
using System;
using System.Collections.Generic;
#nullable enable

namespace production_supply_system.EntityFramework.DAL.TestContext;

public partial class TestContext : DbContext
{
    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbdBoundVin> TbdBoundVins { get; set; }

    public virtual DbSet<TbdCarrier> TbdCarriers { get; set; }

    public virtual DbSet<TbdCase> TbdCases { get; set; }

    public virtual DbSet<TbdContainersInLot> TbdContainersInLots { get; set; }

    public virtual DbSet<TbdCustomsClearance> TbdCustomsClearances { get; set; }

    public virtual DbSet<TbdCustomsPart> TbdCustomsParts { get; set; }

    public virtual DbSet<TbdDocmapper> TbdDocmappers { get; set; }

    public virtual DbSet<TbdDocmapperColumn> TbdDocmapperColumns { get; set; }

    public virtual DbSet<TbdDocmapperContent> TbdDocmapperContents { get; set; }

    public virtual DbSet<TbdHistoryOfChange> TbdHistoryOfChanges { get; set; }

    public virtual DbSet<TbdImoCargo> TbdImoCargos { get; set; }

    public virtual DbSet<TbdInvoice> TbdInvoices { get; set; }

    public virtual DbSet<TbdLocation> TbdLocations { get; set; }

    public virtual DbSet<TbdLog> TbdLogs { get; set; }

    public virtual DbSet<TbdLot> TbdLots { get; set; }

    public virtual DbSet<TbdPartsInContainer> TbdPartsInContainers { get; set; }

    public virtual DbSet<TbdPartsInInvoice> TbdPartsInInvoices { get; set; }

    public virtual DbSet<TbdPlannedSequence> TbdPlannedSequences { get; set; }

    public virtual DbSet<TbdProcess> TbdProcesses { get; set; }

    public virtual DbSet<TbdProcessesStep> TbdProcessesSteps { get; set; }

    public virtual DbSet<TbdPurchaseOrder> TbdPurchaseOrders { get; set; }

    public virtual DbSet<TbdSection> TbdSections { get; set; }

    public virtual DbSet<TbdShipper> TbdShippers { get; set; }

    public virtual DbSet<TbdStatusesForSfsUpload> TbdStatusesForSfsUploads { get; set; }

    public virtual DbSet<TbdTermsOfContainerUse> TbdTermsOfContainerUses { get; set; }

    public virtual DbSet<TbdTermsOfDelivery> TbdTermsOfDeliveries { get; set; }

    public virtual DbSet<TbdTracing> TbdTracings { get; set; }

    public virtual DbSet<TbdTransport> TbdTransports { get; set; }

    public virtual DbSet<TbdTypesOfContainer> TbdTypesOfContainers { get; set; }

    public virtual DbSet<TbdTypesOfLocation> TbdTypesOfLocations { get; set; }

    public virtual DbSet<TbdTypesOfOrder> TbdTypesOfOrders { get; set; }

    public virtual DbSet<TbdTypesOfPacking> TbdTypesOfPackings { get; set; }

    public virtual DbSet<TbdTypesOfPart> TbdTypesOfParts { get; set; }

    public virtual DbSet<TbdTypesOfTransport> TbdTypesOfTransports { get; set; }

    public virtual DbSet<TbdUser> TbdUsers { get; set; }

    public virtual DbSet<TbdVinNumbersLocal> TbdVinNumbersLocals { get; set; }

    public virtual DbSet<TbdVinsInContainer> TbdVinsInContainers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.TbdBoundVinConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdCaseConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdContainersInLotConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdCustomsClearanceConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdCustomsPartConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdDocmapperConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdDocmapperContentConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdHistoryOfChangeConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdInvoiceConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdLocationConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdLotConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdPartsInContainerConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdPartsInInvoiceConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdPlannedSequenceConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdProcessConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdProcessesStepConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdPurchaseOrderConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdSectionConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdShipperConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdStatusesForSfsUploadConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdTermsOfContainerUseConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdTermsOfDeliveryConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdTracingConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdTypesOfPackingConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdTypesOfPartConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdTypesOfTransportConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdUserConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TbdVinsInContainerConfiguration());

        modelBuilder.HasSequence("PPSequence", "Planning").HasMin(1L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
