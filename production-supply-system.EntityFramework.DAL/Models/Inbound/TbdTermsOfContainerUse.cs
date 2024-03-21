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

[Table("tbd_Terms_Of_Container_Use", Schema = "Inbound")]
public partial class TermsOfContainerUse
{
    [Key]
    public int Id { get; set; }

    [Column("Init_Date")]
    public DateOnly InitDate { get; set; }

    [Column("Carrier_Id")]
    public int CarrierId { get; set; }

    public int? Detention { get; set; }

    public int? Storage { get; set; }

    [ForeignKey("CarrierId")]
    [InverseProperty("TermsOfContainerUses")]
    public virtual Carrier Carrier { get; set; } = null!;
}