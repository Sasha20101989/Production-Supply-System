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


namespace production_supply_system.EntityFramework.DAL.Models.PlanningSchema;

[Table("tbd_Planned_Sequence", Schema = "Planning")]
[Index("VinInContainerId", Name = "IX_tbd_Planned_Sequence_VIN_In_Cont", IsUnique = true)]
public partial class PlannedSequence
{
    [Key]
    [Column("Planned_Sequence_Id")]
    public int PlannedSequenceId { get; set; }

    [Column("VIN_In_Container_Id")]
    public int VinInContainerId { get; set; }

    [Column("PP_Order")]
    public int? PpOrder { get; set; }

    [Column("CCR_Order")]
    public int? CcrOrder { get; set; }

    [Column("Status_For_SFS_Upload_Id")]
    public int? StatusForSfsUploadId { get; set; }

    [Column("Is_Suspicious")]
    public int? IsSuspicious { get; set; }

    [Column("Is_Unyelding")]
    public int? IsUnyelding { get; set; }

    [ForeignKey("StatusForSfsUploadId")]
    [InverseProperty("PlannedSequences")]
    public virtual StatusesForSfsUpload? StatusForSfsUpload { get; set; }

    [ForeignKey("VinInContainerId")]
    [InverseProperty("PlannedSequence")]
    public virtual VinsInContainer VinInContainer { get; set; } = null!;
}