using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Types_Of_Packing", Schema = "Inbound")]
public partial class TypesOfPacking
{
    [Key]
    [Column("Packing_Type_Id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Supplier packing type is required")]
    [MaxLength(150, ErrorMessage = "Supplier packing type not exceed 150 characters")]
    [Column("Supplier_Packing_Type")]
    public string? SupplierPackingType { get; set; }

    [Required(ErrorMessage = "Packing type is required")]
    [MaxLength(150, ErrorMessage = "Packing type not exceed 150 characters")]
    [Column("Packing_Type")]
    public string? PackingType { get; set; }
}