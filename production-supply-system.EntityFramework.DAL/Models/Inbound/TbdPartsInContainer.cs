﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using production_supply_system.EntityFramework.DAL.Models.CustomsSchema;
using production_supply_system.EntityFramework.DAL.Models.dboSchema;
using production_supply_system.EntityFramework.DAL.Models.DocmapperSchema;
using production_supply_system.EntityFramework.DAL.Models.InboundSchema;
using production_supply_system.EntityFramework.DAL.Models.MasterSchema;
using production_supply_system.EntityFramework.DAL.Models.PartscontrolSchema;
using production_supply_system.EntityFramework.DAL.Models.PlanningSchema;
using production_supply_system.EntityFramework.DAL.Models.UsersSchema;


namespace production_supply_system.EntityFramework.DAL.Models.InboundSchema;

[Table("tbd_Parts_In_Container", Schema = "Inbound")]
public partial class PartsInContainer
{
    [Key]
    [Column("Part_In_Container_Id")]
    public int PartInContainerId { get; set; }

    [Column("Container_In_Lot_Id")]
    public int ContainerInLotId { get; set; }

    [Column("Case_Id")]
    public int? CaseId { get; set; }

    [Column("Part_Number_Id")]
    public int PartNumberId { get; set; }

    public int Quantity { get; set; }

    [Column("Part_Invoice_Id")]
    public int PartInvoiceId { get; set; }

    [ForeignKey("CaseId")]
    [InverseProperty("PartsInContainers")]
    public virtual Case? Case { get; set; }

    [ForeignKey("ContainerInLotId")]
    [InverseProperty("PartsInContainers")]
    public virtual ContainersInLot ContainerInLot { get; set; } = null!;

    [ForeignKey("PartInvoiceId")]
    [InverseProperty("PartsInContainers")]
    public virtual Invoice PartInvoice { get; set; } = null!;

    [ForeignKey("PartNumberId")]
    [InverseProperty("PartsInContainers")]
    public virtual CustomsPart PartNumber { get; set; } = null!;
}