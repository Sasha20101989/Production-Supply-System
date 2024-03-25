using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.BomContext.Models;

[Table("tbd_Parts")]
public partial class Part
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string PartNumber { get; set; } = null!;

    [StringLength(100)]
    public string? PartName { get; set; }

    [StringLength(50)]
    public string? SupplierPartCode { get; set; }

    [StringLength(100)]
    public string? SupplierPartName { get; set; }

    [StringLength(50)]
    public string? AdditionalPartCode { get; set; }

    [Column("SupplierCodeID")]
    public int? SupplierCodeId { get; set; }

    public int? IntColor { get; set; }

    public int? ExtColor { get; set; }

    [Column("HSCode")]
    public int? Hscode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateAdd { get; set; }

    [Column("PartTypeID")]
    public int? PartTypeId { get; set; }

    [ForeignKey("ExtColor")]
    [InverseProperty("Parts")]
    public virtual ExternalColor? ExtColorNavigation { get; set; }

    [ForeignKey("IntColor")]
    [InverseProperty("Parts")]
    public virtual InternalColor? IntColorNavigation { get; set; }

    [ForeignKey("PartTypeId")]
    [InverseProperty("Parts")]
    public virtual PartsType? PartType { get; set; }

    [ForeignKey("SupplierCodeId")]
    [InverseProperty("Parts")]
    public virtual Supplier? SupplierCode { get; set; }
}