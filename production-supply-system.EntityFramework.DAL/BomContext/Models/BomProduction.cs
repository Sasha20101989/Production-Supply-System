using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.BomContext.Models;

[Table("tbd_BomProduction")]
public partial class BomProduction
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ModelVariantID")]
    public int ModelVariantId { get; set; }

    [Column("PartsApplicationID")]
    public int PartsApplicationId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateAdd { get; set; }

    [ForeignKey("ModelVariantId")]
    [InverseProperty("BomProductions")]
    public virtual ModVar ModelVariant { get; set; } = null!;

    [ForeignKey("PartsApplicationId")]
    [InverseProperty("BomProductions")]
    public virtual PartsApplication PartsApplication { get; set; } = null!;
}