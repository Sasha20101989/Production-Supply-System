using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Contracts;

namespace DAL.Models.Planning
{
    [Table("tbd_Bound_VINs", Schema = "Planning")]
    public partial class BoundVin : IEntity
    {
        [Key]
        [Column("Bound_VINs_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "VIN In Container Id is required.")]
        [Column("VIN_In_Container_Id")]
        public int VinInContainerId { get; set; }

        [Required(ErrorMessage = "VIN Number Local Id is required.")]
        [Column("VIN_Number_Local_Id")]
        public int VinNumberLocalId { get; set; }

        [ForeignKey("VinInContainerId")]
        [InverseProperty("BoundVin")]
        public virtual VinsInContainer VinInContainer { get; set; } = null!;

        [ForeignKey("VinNumberLocalId")]
        [InverseProperty("BoundVin")]
        public virtual VinNumbersLocal VinNumberLocal { get; set; } = null!;
    }
}
