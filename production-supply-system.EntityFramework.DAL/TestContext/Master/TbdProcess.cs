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


namespace production_supply_system.EntityFramework.DAL.TestContext.MasterSchema;

[Table("tbd_Processes", Schema = "Master")]
public partial class TbdProcess
{
    [Key]
    [Column("Process_Id")]
    public int ProcessId { get; set; }

    [Column("Process_Name")]
    [StringLength(300)]
    public string ProcessName { get; set; } = null!;

    [InverseProperty("Process")]
    public virtual ICollection<TbdProcessesStep> TbdProcessesSteps { get; set; } = new List<TbdProcessesStep>();
}