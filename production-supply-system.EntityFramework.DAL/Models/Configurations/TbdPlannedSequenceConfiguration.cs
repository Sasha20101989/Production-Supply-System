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
    public partial class PlannedSequenceConfiguration : IEntityTypeConfiguration<PlannedSequence>
    {
        public void Configure(EntityTypeBuilder<PlannedSequence> entity)
        {
            entity.Property(e => e.IsSuspicious).HasDefaultValue(0);
            entity.Property(e => e.IsUnyelding).HasDefaultValue(0);
            entity.Property(e => e.StatusForSfsUploadId).HasDefaultValue(1);

            entity.HasOne(d => d.StatusForSfsUpload).WithMany(p => p.PlannedSequences).HasConstraintName("FK_tbd_PlannedSequence_tbd_StatusesForSFSUpload");

            entity.HasOne(d => d.VinInContainer).WithOne(p => p.PlannedSequence)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_Planned_Sequence_tbd_VINs_In_Container");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PlannedSequence> entity);
    }
}
