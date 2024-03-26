using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using production_supply_system.EntityFramework.DAL.TestContext.InboundSchema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Invoices", Schema = "Inbound")]
[Index("InvoiceNumber", Name = "IX_tbd_Invoices", IsUnique = true)]
public partial class Invoice
{
    [Key]
    [Column("Invoice_Id")]
    public int Id { get; set; }

    [Column("Invoice_Number")]
    [MaxLength(20, ErrorMessage = "Invoice number must not exceed 20 characters.")]
    public string? InvoiceNumber { get; set; }

    [Column("Invoice_Date")]
    public DateTime InvoiceDate { get; set; }

    [Column("Shipper_Id")]
    public int ShipperId { get; set; }

    [Column("Purchase_Order_Id")]
    public int? PurchaseOrderId { get; set; }

    [ForeignKey("PurchaseOrderId")]
    public virtual PurchaseOrder? PurchaseOrder { get; set; }

    [ForeignKey("ShipperId")]
    public virtual Shipper? Shipper { get; set; }

    [InverseProperty("Invoice")]
    public virtual ICollection<PartsInInvoice> PartsInInvoices { get; set; } = [];
}