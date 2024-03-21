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


namespace production_supply_system.EntityFramework.DAL.Models.CustomsSchema;

[Table("tbd_Customs_Parts", Schema = "Customs")]
[Index("PartNumberId", Name = "IX_tbd_CustomsParts", IsUnique = true)]
public partial class CustomsPart
{
    [Key]
    [Column("Part_Number_Id")]
    public int PartNumberId { get; set; }

    [Column("Part_Number")]
    [StringLength(50)]
    public string PartNumber { get; set; } = null!;

    [Column("Part_Name_Eng")]
    [StringLength(150)]
    public string PartNameEng { get; set; } = null!;

    [Column("Part_Name_Rus")]
    public string? PartNameRus { get; set; }

    [Column("Hs_Code")]
    [StringLength(50)]
    public string? HsCode { get; set; }

    [Column("Part_Type_Id")]
    public int PartTypeId { get; set; }

    [Column("Date_Add", TypeName = "datetime")]
    public DateTime? DateAdd { get; set; }

    [ForeignKey("PartTypeId")]
    [InverseProperty("CustomsParts")]
    public virtual TypesOfPart PartType { get; set; } = null!;

    [InverseProperty("PartNumber")]
    public virtual ICollection<PartsInContainer> PartsInContainers { get; set; } = new List<PartsInContainer>();

    [InverseProperty("PartNumber")]
    public virtual ICollection<PartsInInvoice> PartsInInvoices { get; set; } = new List<PartsInInvoice>();
}