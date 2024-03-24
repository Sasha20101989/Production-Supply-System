using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Parts_In_Invoice", Schema = "Inbound")]
public partial class PartsInInvoice
{
    [Key]
    [Column("Part_In_Invoice_Id")]
    public int PartInInvoiceId { get; set; }

    [Column("Invoice_Id")]
    public int InvoiceId { get; set; }

    [Column("Part_Number_Id")]
    public int PartNumberId { get; set; }

    [Column(TypeName = "decimal(8, 3)")]
    public decimal Quantity { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal Price { get; set; }

    [ForeignKey("InvoiceId")]
    public virtual Invoice Invoice { get; set; } = null!;

    [ForeignKey("PartNumberId")]
    public virtual CustomsPart PartNumber { get; set; } = null!;
}