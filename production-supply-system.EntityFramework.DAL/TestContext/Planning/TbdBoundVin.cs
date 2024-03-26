﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using production_supply_system.EntityFramework.DAL.TestContext.CustomsSchema;
using production_supply_system.EntityFramework.DAL.TestContext.dboSchema;
using production_supply_system.EntityFramework.DAL.TestContext.DocmapperSchema;
using production_supply_system.EntityFramework.DAL.TestContext.InboundSchema;
using production_supply_system.EntityFramework.DAL.TestContext.MasterSchema;
using production_supply_system.EntityFramework.DAL.TestContext.PartscontrolSchema;
using production_supply_system.EntityFramework.DAL.TestContext.PlanningSchema;
using production_supply_system.EntityFramework.DAL.TestContext.UsersSchema;


namespace production_supply_system.EntityFramework.DAL.TestContext.PlanningSchema;

[Table("tbd_Bound_VINs", Schema = "Planning")]
[Index("VinInContainerId", Name = "IX_tbd_Bound_VINs_Uniq_VIN_In_Cont", IsUnique = true)]
[Index("VinNumberLocalId", Name = "IX_tbd_Bound_VINs_Uniq_VIN_Number_Local", IsUnique = true)]
public partial class TbdBoundVin
{
    [Key]
    [Column("Bound_VINs_Id")]
    public int BoundVinsId { get; set; }

    [Column("VIN_In_Container_Id")]
    public int VinInContainerId { get; set; }

    [Column("VIN_Number_Local_Id")]
    public int VinNumberLocalId { get; set; }

    [ForeignKey("VinInContainerId")]
    [InverseProperty("TbdBoundVin")]
    public virtual TbdVinsInContainer VinInContainer { get; set; } = null!;

    [ForeignKey("VinNumberLocalId")]
    [InverseProperty("TbdBoundVin")]
    public virtual TbdVinNumbersLocal VinNumberLocal { get; set; } = null!;
}