using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Attributes;
using DAL.Extensions;
using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о детали в контейнере.
    /// </summary>
    [Table("tbd_Parts_In_Container", Schema = "Inbound")]
    public class PartsInContainer : IEntity
    {
        private Case _case;
        private CustomsPart _partNumber;
        private ContainersInLot _containerInLot;
        private Invoice _partInvoice;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Part_In_Container_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Column("Quantity", TypeName = "decimal(8, 3)")]
        public decimal Quantity { get; set; }

        [Column("Case_Id")]
        public int? CaseId { get; set; }

        [ForeignKey("CaseId")]
        public virtual Case Case
        {
            get => _case;
            set
            {
                _case = value;
                CaseId = value?.Id ?? null;
            }
        }

        [Required(ErrorMessage = "Container In Lot Id is required.")]
        [Column("Container_In_Lot_Id")]
        [Min(1)]
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

        [Required(ErrorMessage = "Part Invoice Id is required.")]
        [Column("Part_Invoice_Id")]
        [Min(1)]
        public int PartInvoiceId { get; set; }

        [ForeignKey("PartInvoiceId")]
        public virtual Invoice PartInvoice
        {
            get => _partInvoice;
            set
            {
                _partInvoice = value;
                PartInvoiceId = value?.Id ?? 0;
            }
        }

        [Required(ErrorMessage = "Part Number Id is required.")]
        [Column("Part_Number_Id")]
        [Min(1)]
        public int PartNumberId { get; set; }

        [ForeignKey("PartNumberId")]
        public virtual CustomsPart PartNumber
        {
            get => _partNumber;
            set
            {
                _partNumber = value;
                PartNumberId = value?.Id ?? 0;
            }
        }
    }
}
