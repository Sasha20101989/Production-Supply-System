using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Types_Of_Packing", Schema = "Inbound")]
public partial class TypesOfPacking
{
    [Key]
    [Column("Packing_Type_Id")]
    public int PackingTypeId { get; set; }

    [Column("Supplier_Packing_Type")]
    [StringLength(150)]
    public string? SupplierPackingType { get; set; }

    [Column("Packing_Type")]
    [StringLength(150)]
    public string? PackingType { get; set; }
}