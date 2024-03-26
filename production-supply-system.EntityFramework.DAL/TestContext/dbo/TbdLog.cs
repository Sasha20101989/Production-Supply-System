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


namespace production_supply_system.EntityFramework.DAL.TestContext.dboSchema;

[Table("tbd_Log")]
public partial class TbdLog
{
    [Key]
    [Column("Log_Id")]
    public long LogId { get; set; }

    [Column("Machine_Name")]
    [StringLength(200)]
    public string MachineName { get; set; } = null!;

    [Column("Log_Date", TypeName = "datetime")]
    public DateTime LogDate { get; set; }

    [Column("Log_Level")]
    [StringLength(5)]
    public string LogLevel { get; set; } = null!;

    [Column("Log_Message")]
    public string LogMessage { get; set; } = null!;

    [Column("Domain_User")]
    [StringLength(300)]
    public string DomainUser { get; set; } = null!;

    [StringLength(300)]
    public string Logger { get; set; } = null!;
}