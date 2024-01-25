using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о детали в счёте.
    /// </summary>
    [Table("tbd_Parts_In_Invoice", Schema = "Inbound")]
    public class PartInInvoice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Part_In_Invoice_Id")]
        public int PartInInvoiceId { get; set; }

        [Required]
        [Column("Invoice_Id")]
        public int InvoiceId { get; set; }

        [Required]
        [Column("Part_Number_Id")]
        public int PartNumberId { get; set; }

        public decimal? Quantity { get; set; }

        [Column("Init_Price")]
        public decimal? InitPrice { get; set; }

        [ForeignKey("Invoice_Id")]
        public virtual Invoice Invoice { get; set; }

        [ForeignKey("Part_Number_Id")]
        public virtual CustomsPart CustomsPart { get; set; }
    }
}
