using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace production_supply_system.EntityFramework.DAL.BomContext.Models;

[Table("tbd_ModelVariants")]
[Index("DrawingNumber", Name = "AK_DwgNr", IsUnique = true)]
[Index("ModelVariant", Name = "AK_ModVar", IsUnique = true)]
public partial class ModVar
{
    [Key]
    public int Id { get; set; }

    [Column("EndItemID")]
    public int EndItemId { get; set; }

    [Column("IntColID")]
    public int IntColId { get; set; }

    [Column("ExtColID")]
    public int ExtColId { get; set; }

    [StringLength(14)]
    public string DrawingNumber { get; set; } = null!;

    [StringLength(14)]
    [Column("ModelVariant")]
    public string ModelVariant { get; set; } = null!;

    [StringLength(50)]
    public string? SupplierEndItem { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModVarAdopt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModVarAbolish { get; set; }

    [ForeignKey("EndItemId")]
    [InverseProperty("ModelVariants")]
    public virtual EndItem EndItem { get; set; } = null!;

    [ForeignKey("ExtColId")]
    [InverseProperty("ModelVariants")]
    public virtual ExternalColor ExtCol { get; set; } = null!;

    [ForeignKey("IntColId")]
    [InverseProperty("ModelVariants")]
    public virtual InternalColor IntCol { get; set; } = null!;

    [InverseProperty("ModelVariant")]
    public virtual ICollection<BomProduction> BomProductions { get; set; } = [];
}