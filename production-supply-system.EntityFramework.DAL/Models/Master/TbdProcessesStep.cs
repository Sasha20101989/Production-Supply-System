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


namespace production_supply_system.EntityFramework.DAL.Models.MasterSchema;

[Table("tbd_Processes_Steps", Schema = "Master")]
public partial class ProcessesStep
{
    [Key]
    [Column("Process_Step_Id")]
    public int ProcessStepId { get; set; }

    [Column("Process_Id")]
    public int ProcessId { get; set; }

    public int Step { get; set; }

    [Column("Docmapper_Id")]
    public int DocmapperId { get; set; }

    [Column("Section_Id")]
    public int SectionId { get; set; }

    [Column("Step_Name")]
    [StringLength(50)]
    public string StepName { get; set; } = null!;

    [ForeignKey("DocmapperId")]
    [InverseProperty("ProcessesSteps")]
    public virtual Docmapper Docmapper { get; set; } = null!;

    [ForeignKey("ProcessId")]
    [InverseProperty("ProcessesSteps")]
    public virtual Process Process { get; set; } = null!;

    [ForeignKey("SectionId")]
    [InverseProperty("ProcessesSteps")]
    public virtual Section Section { get; set; } = null!;
}