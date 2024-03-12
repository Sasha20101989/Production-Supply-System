using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Attributes;
using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о счёте.
    /// </summary>
    [Table("tbd_Invoices", Schema = "Inbound")]
    public class Invoice : IEntity
    {
        private PurchaseOrder _purchaseOrder;
        private Shipper _shipper;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Invoice_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Invoice Number is required.")]
        [MaxLength(20, ErrorMessage = "Invoice Number must not exceed 20 characters.")]
        [ConcurrencyCheck]
        [Column("Invoice_Number")]
        public string InvoiceNumber { get; set; } = "-";

        [Required(ErrorMessage = "Invoice Date is required.")]
        [Column("Invoice_Date")]
        public DateTime InvoiceDate { get; set; }

        [Column("Purchase_Order_Id")]
        public int? PurchaseOrderId { get; set; }

        [ForeignKey("LotInvoiceId")]
        public virtual PurchaseOrder PurchaseOrder
        {
            get => _purchaseOrder;
            set
            {
                _purchaseOrder = value;
                PurchaseOrderId = value?.Id ?? null;
            }
        }

        [Required(ErrorMessage = "Shipper Id is required.")]
        [Column("Shipper_Id")]
        [Min(1)]
        public int ShipperId { get; set; }

        [ForeignKey("ShipperId")]
        public virtual Shipper Shipper
        {
            get => _shipper;
            set
            {
                _shipper = value;
                ShipperId = value?.Id ?? 0;
            }
        }
    }
}
