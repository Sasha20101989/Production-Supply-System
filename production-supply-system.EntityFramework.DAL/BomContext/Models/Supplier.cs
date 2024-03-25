using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.BomContext.Models;

[Table("tbd_Suppliers")]
public partial class Supplier
{
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    public string SupplierCode { get; set; } = null!;

    [StringLength(50)]
    public string SupplierName { get; set; } = null!;

    [StringLength(10)]
    public string SupplierCountryCode { get; set; } = null!;

    [InverseProperty("SupplierCode")]
    public virtual ICollection<Part> Parts { get; set; } = [];
}