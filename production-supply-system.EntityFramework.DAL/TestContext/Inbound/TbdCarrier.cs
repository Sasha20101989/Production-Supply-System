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


namespace production_supply_system.EntityFramework.DAL.TestContext.InboundSchema;

[Table("tbd_Carriers", Schema = "Inbound")]
public partial class TbdCarrier
{
    [Key]
    [Column("Carrier_Id")]
    public int CarrierId { get; set; }

    [Column("Carrier_Name")]
    [StringLength(50)]
    public string CarrierName { get; set; } = null!;

    [InverseProperty("Carrier")]
    public virtual ICollection<TbdLot> TbdLots { get; set; } = new List<TbdLot>();

    [InverseProperty("Carrier")]
    public virtual ICollection<TbdTermsOfContainerUse> TbdTermsOfContainerUses { get; set; } = new List<TbdTermsOfContainerUse>();

    [InverseProperty("Carrier")]
    public virtual ICollection<TbdTracing> TbdTracings { get; set; } = new List<TbdTracing>();
}