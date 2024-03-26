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
    public partial class TbdTypesOfPartConfiguration : IEntityTypeConfiguration<TbdTypesOfPart>
    {
        public void Configure(EntityTypeBuilder<TbdTypesOfPart> entity)
        {
            entity.HasKey(e => e.PartTypeId).HasName("PK_tbd_PartTypes");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TbdTypesOfPart> entity);
    }
}
