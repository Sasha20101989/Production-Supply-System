﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using production_supply_system.EntityFramework.DAL.Models;
using production_supply_system.EntityFramework.DAL.Models.CustomsSchema;
using production_supply_system.EntityFramework.DAL.Models.dboSchema;
using production_supply_system.EntityFramework.DAL.Models.DocmapperSchema;
using production_supply_system.EntityFramework.DAL.Models.InboundSchema;
using production_supply_system.EntityFramework.DAL.Models.MasterSchema;
using production_supply_system.EntityFramework.DAL.Models.PartscontrolSchema;
using production_supply_system.EntityFramework.DAL.Models.PlanningSchema;
using production_supply_system.EntityFramework.DAL.Models.UsersSchema;
using System;
using System.Collections.Generic;

#nullable disable

namespace production_supply_system.EntityFramework.DAL.Models.Configurations
{
    public partial class PartsInInvoiceConfiguration : IEntityTypeConfiguration<PartsInInvoice>
    {
        public void Configure(EntityTypeBuilder<PartsInInvoice> entity)
        {
            entity.HasKey(e => e.PartInInvoiceId).HasName("PK_tbd_PartsInInvoice");

            entity.HasOne(d => d.Invoice).WithMany(p => p.PartsInInvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_PartsInInvoice_tbd_Invoices1");

            entity.HasOne(d => d.PartNumber).WithMany(p => p.PartsInInvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_PartsInInvoice_tbd_CustomsParts");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PartsInInvoice> entity);
    }
}
