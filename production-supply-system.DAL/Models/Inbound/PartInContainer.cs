using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о детали в контейнере.
    /// </summary>
    [Table("tbd_Parts_In_Container", Schema = "Inbound")]
    public class PartInContainer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Part_In_Container_Id")]
        public int PartInContainerId { get; set; }

        [Required]
        [Column("Container_In_Lot_Id")]
        public int ContainerInLotId { get; set; }

        [Column("Case_Id")]
        public int? CaseId { get; set; }

        [Required]
        [Column("Part_Number_Id")]
        public int PartNumberId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column("Part_Invoice_Id")]
        public int PartInvoiceId { get; set; }

        [ForeignKey("Container_In_Lot_Id")]
        public virtual ContainerInLot ContainerInLot { get; set; }

        [ForeignKey("Case_Id")]
        public virtual Case Case { get; set; }

        [ForeignKey("Part_Number_Id")]
        public virtual CustomsPart CustomsPart { get; set; }
    }
}
