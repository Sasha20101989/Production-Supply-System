using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DAL.Models.Contracts;

namespace DAL.Models.Planning
{
    [Table("_Planned_Sequence", Schema = "Planning")]
    public partial class PlannedSequence : IEntity
    {
        [Key]
        [Column("Planned_Sequence_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "VIN In Container Id is required.")]
        [Column("VIN_In_Container_Id")]
        public int VinInContainerId { get; set; }

        [Column("PP_Order")]
        public int? PpOrder { get; set; }

        [Column("CCR_Order")]
        public int? CcrOrder { get; set; }

        [Column("Status_For_SFS_Upload_Id", TypeName = "int")]
        [DefaultValue(1)]
        public int? StatusForSfsUploadId { get; set; }

        [Column("Is_Suspicious", TypeName = "int")]
        [DefaultValue(0)]
        public int? IsSuspicious { get; set; }

        [Column("Is_Unyelding", TypeName = "int")]
        [DefaultValue(0)]
        public int? IsUnyelding { get; set; }

        [ForeignKey("StatusForSfsUploadId")]
        public virtual StatusesForSfsUpload? StatusForSfsUpload { get; set; }

        [ForeignKey("VinInContainerId")]
        public virtual VinsInContainer VinInContainer { get; set; } = null!;
    }
}
