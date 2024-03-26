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
    public partial class TbdTermsOfContainerUseConfiguration : IEntityTypeConfiguration<TbdTermsOfContainerUse>
    {
        public void Configure(EntityTypeBuilder<TbdTermsOfContainerUse> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK_tbd_TracingEventStaticData");

            entity.HasOne(d => d.Carrier).WithMany(p => p.TbdTermsOfContainerUses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbd_Terms_Of_Container_Use_tbd_Carriers");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TbdTermsOfContainerUse> entity);
    }
}
