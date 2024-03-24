using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Purchase_Orders", Schema = "Partscontrol")]
[Index("PurchaseOrderNumber", Name = "IX_tbd_PurchaseOrders", IsUnique = true)]
public partial class PurchaseOrder
{
    [Key]
    [Column("Purchase_Order_Id")]
    public int PurchaseOrderId { get; set; }

    [Column("Shipper_Id")]
    public int ShipperId { get; set; }

    [Column("Order_Type_Id")]
    public int OrderTypeId { get; set; }

    [Column("Purchase_Order_Number")]
    [StringLength(10)]
    public string PurchaseOrderNumber { get; set; } = null!;

    [Column("Purchase_Order_Date")]
    public DateOnly PurchaseOrderDate { get; set; }

    [ForeignKey("OrderTypeId")]
    public virtual TypesOfOrder OrderType { get; set; } = null!;

    [ForeignKey("ShipperId")]
    public virtual Shipper Shipper { get; set; } = null!;
}