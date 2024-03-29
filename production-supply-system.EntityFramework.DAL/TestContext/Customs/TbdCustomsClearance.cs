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


namespace production_supply_system.EntityFramework.DAL.TestContext.CustomsSchema;

[Table("tbd_Customs_Clearance", Schema = "Customs")]
public partial class TbdCustomsClearance
{
    [Key]
    [Column("Customs_Clearance_Id")]
    public int CustomsClearanceId { get; set; }

    [Column("Container_In_Lot_Id")]
    public int ContainerInLotId { get; set; }

    [Column("Invoce_Number")]
    [StringLength(50)]
    public string? InvoceNumber { get; set; }

    [Column("Docs_To_Customs_Date")]
    public DateOnly? DocsToCustomsDate { get; set; }

    [Column("AEO_Obbligation_Number")]
    [StringLength(20)]
    public string? AeoObbligationNumber { get; set; }

    [Column("AEO_Obbligation_Release_Date")]
    public DateOnly? AeoObbligationReleaseDate { get; set; }

    [Column("CCD_Number")]
    [StringLength(20)]
    public string? CcdNumber { get; set; }

    [Column("CCD_Release_Date")]
    public DateOnly? CcdReleaseDate { get; set; }

    [Column("Customs_Inpection_Need")]
    public bool? CustomsInpectionNeed { get; set; }

    [Column("EDocuments_To_Be_Provided_Date")]
    public DateOnly? EdocumentsToBeProvidedDate { get; set; }

    [Column("EDocuments_To_Be_Received_Date")]
    public DateOnly? EdocumentsToBeReceivedDate { get; set; }

    [Column("Part_Type_Id")]
    public int? PartTypeId { get; set; }

    [ForeignKey("ContainerInLotId")]
    [InverseProperty("TbdCustomsClearances")]
    public virtual TbdContainersInLot ContainerInLot { get; set; } = null!;

    [ForeignKey("PartTypeId")]
    [InverseProperty("TbdCustomsClearances")]
    public virtual TbdTypesOfPart? PartType { get; set; }
}