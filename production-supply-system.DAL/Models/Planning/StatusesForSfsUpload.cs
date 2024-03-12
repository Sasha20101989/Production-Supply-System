using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Contracts;

namespace DAL.Models.Planning
{
    [Table("tbd_Statuses_For_SFS_Upload", Schema = "Planning")]
    public partial class StatusesForSfsUpload : IEntity
    {
        [Key]
        [Column("Status_For_SFS_Upload_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Status Name is required.")]
        [MaxLength(10, ErrorMessage = "Status Name must not exceed 10 characters.")]
        [Column("Status_Name")]
        public string StatusName { get; set; } = null!;

        public virtual ICollection<PlannedSequence> PlannedSequences { get; set; }
    }
}
