using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

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