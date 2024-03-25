using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Shippers", Schema = "Inbound")]
public partial class Shipper
{
    [Key]
    [Column("Shipper_Id")]
    public int Id { get; set; }

    [Column("Shipper_Name")]
    [StringLength(20)]
    public string ShipperName { get; set; } = null!;

    [Column("Shipper_Full_Name")]
    [StringLength(100)]
    public string? ShipperFullName { get; set; }

    [Column("Shipper_Default_Delivery_Location_Id")]
    public int? ShipperDefaultDeliveryLocationId { get; set; }

    [ForeignKey("ShipperDefaultDeliveryLocationId")]
    public virtual Location? ShipperDefaultDeliveryLocation { get; set; }

    [InverseProperty("Shipper")]
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = [];
}