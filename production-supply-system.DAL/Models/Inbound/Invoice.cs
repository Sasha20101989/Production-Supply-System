using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о счёте.
    /// </summary>
    [Table("tbd_Invoices", Schema = "Inbound")]
    public class Invoice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Invoice_Id")]
        public int InvoiceId { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("Invoice_Number")]
        public string InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        [Column("Invoice_Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [Column("Shipper_Id")]
        public int ShipperId { get; set; }

        [ForeignKey("Shipper_Id")]
        public virtual Shipper Shipper { get; set; }
    }
}
