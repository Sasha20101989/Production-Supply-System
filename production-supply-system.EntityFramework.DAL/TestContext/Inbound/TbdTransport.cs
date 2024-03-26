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

[Table("tbd_Transports", Schema = "Inbound")]
[Index("TransportName", Name = "IX_tbd_Transports", IsUnique = true)]
public partial class TbdTransport
{
    [Key]
    [Column("Transport_Id")]
    public int TransportId { get; set; }

    [Column("Transport_Name")]
    [StringLength(50)]
    public string TransportName { get; set; } = null!;

    [InverseProperty("LotTransport")]
    public virtual ICollection<TbdLot> TbdLots { get; set; } = new List<TbdLot>();

    [InverseProperty("TraceTransport")]
    public virtual ICollection<TbdTracing> TbdTracings { get; set; } = new List<TbdTracing>();
}