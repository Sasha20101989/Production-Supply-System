using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Types_Of_Order", Schema = "Partscontrol")]
public partial class TypesOfOrder
{
    [Key]
    [Column("Order_Type_Id")]
    public int OrderTypeId { get; set; }

    [Column("Order_Type")]
    [StringLength(10)]
    public string? OrderType { get; set; }
}