using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Contracts;
using DAL.Attributes;

namespace DAL.Models.Planning
{
    [Table("tbd_VINs_In_Container", Schema = "Planning")]
    public partial class VinsInContainer : IEntity
    {
        private Lot _lot;
        private ContainersInLot _containerInLot;
        private BodyModelVariant _modvar;

        [Key]
        [Column("VIN_In_Container_Id")]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "The length of Supplier VIN Number should not exceed 50 characters.")]
        [Column("Supplier_VIN_Number")]
        public string SupplierVinNumber { get; set; }

        [Required(ErrorMessage = "Lot Id is required.")]
        [Column("Lot_Id")]
        [Min(1)]
        public int LotId { get; set; }

        [ForeignKey("LotId")]
        public virtual Lot Lot
        {
            get => _lot;
            set
            {
                _lot = value;
                LotId = value?.Id ?? 0;
            }
        }

        [Required(ErrorMessage = "Container In Lot ID is required.")]
        [Column("Container_In_Lot_Id")]
        public int ContainerInLotId { get; set; }

        [ForeignKey("ContainerInLotId")]
        public virtual ContainersInLot ContainerInLot
        {
            get => _containerInLot;
            set
            {
                _containerInLot = value;
                ContainerInLotId = value?.Id ?? 0;
            }
        }

        [Required(ErrorMessage = "Model variant Id is required.")]
        [Column("Modvar_Id")]
        [Min(1)]
        public int ModvarId { get; set; }



        [ForeignKey("ModvarId")]
        public virtual BodyModelVariant Modvar
        {
            get => _modvar;
            set
            {
                _modvar = value;
                ModvarId = value?.ModelVariantId ?? 0;
            }
        }

        public virtual string PartNumber { get; set; }

        public virtual TypesOfPart PartType { get; set; }

        public virtual BoundVin? BoundVin { get; set; }

        public virtual PlannedSequence? PlannedSequence { get; set; }
    }
}
