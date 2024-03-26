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
    public partial class TbdCustomsClearanceConfiguration : IEntityTypeConfiguration<TbdCustomsClearance>
    {
        public void Configure(EntityTypeBuilder<TbdCustomsClearance> entity)
        {
            entity.HasOne(d => d.ContainerInLot).WithMany(p => p.TbdCustomsClearances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_Customs_Clearance_tbd_Containers_In_Lot");

            entity.HasOne(d => d.PartType).WithMany(p => p.TbdCustomsClearances).HasConstraintName("FK_tbd_Customs_Clearance_tbd_Types_Of_Part");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TbdCustomsClearance> entity);
    }
}
