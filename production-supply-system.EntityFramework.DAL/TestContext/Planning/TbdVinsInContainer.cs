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

[Table("tbd_VINs_In_Container", Schema = "Planning")]
public partial class TbdVinsInContainer
{
    [Key]
    [Column("VIN_In_Container_Id")]
    public int VinInContainerId { get; set; }

    [Column("Container_In_Lot_Id")]
    public int ContainerInLotId { get; set; }

    [Column("Supplier_VIN_Number")]
    [StringLength(50)]
    public string SupplierVinNumber { get; set; } = null!;

    [Column("Modvar_Id")]
    public int ModvarId { get; set; }

    [Column("Lot_Id")]
    public int LotId { get; set; }

    [ForeignKey("ContainerInLotId")]
    [InverseProperty("TbdVinsInContainers")]
    public virtual TbdContainersInLot ContainerInLot { get; set; } = null!;

    [InverseProperty("VinInContainer")]
    public virtual TbdBoundVin? TbdBoundVin { get; set; }

    [InverseProperty("VinInContainer")]
    public virtual TbdPlannedSequence? TbdPlannedSequence { get; set; }
}