﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using production_supply_system.EntityFramework.DAL.TestContext;
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

#nullable disable

namespace production_supply_system.EntityFramework.DAL.TestContext.Configurations
{
    public partial class TbdProcessesStepConfiguration : IEntityTypeConfiguration<TbdProcessesStep>
    {
        public void Configure(EntityTypeBuilder<TbdProcessesStep> entity)
        {
            entity.HasKey(e => e.ProcessStepId).HasName("PK_Processes_Steps");

            entity.HasOne(d => d.Docmapper).WithMany(p => p.TbdProcessesSteps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Processes_Steps_tbd_Docmapper");

            entity.HasOne(d => d.Process).WithMany(p => p.TbdProcessesSteps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Processes_Steps_Processes");

            entity.HasOne(d => d.Section).WithMany(p => p.TbdProcessesSteps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Processes_Steps_Sections");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TbdProcessesStep> entity);
    }
}
