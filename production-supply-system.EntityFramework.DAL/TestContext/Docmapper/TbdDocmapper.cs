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


namespace production_supply_system.EntityFramework.DAL.TestContext.DocmapperSchema;

[Table("tbd_Docmapper", Schema = "Docmapper")]
public partial class TbdDocmapper
{
    [Key]
    [Column("Docmapper_Id")]
    public int DocmapperId { get; set; }

    [Column("Docmapper_Name")]
    [StringLength(50)]
    public string DocmapperName { get; set; } = null!;

    [Column("Default_Folder")]
    [StringLength(100)]
    public string? DefaultFolder { get; set; }

    [Column("Sheet_Name")]
    [StringLength(50)]
    public string SheetName { get; set; } = null!;

    [Column("First_Data_Row")]
    public int? FirstDataRow { get; set; }

    [Column("Is_Active")]
    public bool IsActive { get; set; }

    [InverseProperty("Docmapper")]
    public virtual ICollection<TbdDocmapperContent> TbdDocmapperContents { get; set; } = new List<TbdDocmapperContent>();

    [InverseProperty("Docmapper")]
    public virtual ICollection<TbdProcessesStep> TbdProcessesSteps { get; set; } = new List<TbdProcessesStep>();
}