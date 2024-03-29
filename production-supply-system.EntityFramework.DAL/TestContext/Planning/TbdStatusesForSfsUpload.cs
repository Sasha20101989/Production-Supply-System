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


namespace production_supply_system.EntityFramework.DAL.TestContext.PlanningSchema;

[Table("tbd_Statuses_For_SFS_Upload", Schema = "Planning")]
public partial class TbdStatusesForSfsUpload
{
    [Key]
    [Column("Status_For_SFS_Upload_Id")]
    public int StatusForSfsUploadId { get; set; }

    [Column("Status_Name")]
    [StringLength(10)]
    public string StatusName { get; set; } = null!;

    [InverseProperty("StatusForSfsUpload")]
    public virtual ICollection<TbdPlannedSequence> TbdPlannedSequences { get; set; } = new List<TbdPlannedSequence>();
}