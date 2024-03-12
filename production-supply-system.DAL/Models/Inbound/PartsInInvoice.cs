using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о детали в счёте.
    /// </summary>
    [Table("tbd_Parts_In_Invoice", Schema = "Inbound")]
    public class PartsInInvoice : IEntity
    {
        private Invoice _invoice;
        private CustomsPart _partNumber;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Part_In_Invoice_Id")]
        public int Id { get; set; }      
        
        [Required(ErrorMessage = "Quantity is required.")]
        [Column("Quantity", TypeName = "decimal(8, 3)")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Initial Price is required.")]
        [Column("Init_Price", TypeName = "decimal(10, 4)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Invoice Id is required.")]
        [Column("Invoice_Id")]
        public int InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice
        {
            get => _invoice;
            set
            {
                _invoice = value;
                InvoiceId = value?.Id ?? 0;
            }
        }

        [Required(ErrorMessage = "Part Number Id is required.")]
        [Column("Part_Number_Id")]
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
