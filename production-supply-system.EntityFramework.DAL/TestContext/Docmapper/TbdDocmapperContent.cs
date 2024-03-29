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

[Table("tbd_Docmapper_Content", Schema = "Docmapper")]
public partial class TbdDocmapperContent
{
    [Key]
    [Column("Docmapper_Content_Id")]
    public int DocmapperContentId { get; set; }

    [Column("Docmapper_Id")]
    public int DocmapperId { get; set; }

    [Column("Docmapper_Column_Id")]
    public int DocmapperColumnId { get; set; }

    [Column("Row_Nr")]
    public int? RowNr { get; set; }

    [Column("Column_Nr")]
    public int ColumnNr { get; set; }

    [ForeignKey("DocmapperId")]
    [InverseProperty("TbdDocmapperContents")]
    public virtual TbdDocmapper Docmapper { get; set; } = null!;

    [ForeignKey("DocmapperColumnId")]
    [InverseProperty("TbdDocmapperContents")]
    public virtual TbdDocmapperColumn DocmapperColumn { get; set; } = null!;
}