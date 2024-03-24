using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Statuses_For_SFS_Upload", Schema = "Planning")]
public partial class StatusesForSfsUpload
{
    [Key]
    [Column("Status_For_SFS_Upload_Id")]
    public int StatusForSfsUploadId { get; set; }

    [Column("Status_Name")]
    [StringLength(10)]
    public string StatusName { get; set; } = null!;

    [InverseProperty("StatusForSfsUpload")]
    public virtual ICollection<PlannedSequence> PlannedSequences { get; set; } = new List<PlannedSequence>();
}