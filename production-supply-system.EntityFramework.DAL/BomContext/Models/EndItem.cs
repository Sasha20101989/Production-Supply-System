using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace production_supply_system.EntityFramework.DAL.BomContext.Models;

[Table("tbd_EndItems")]
[Index("InternalEndItem", Name = "AK_EndItem", IsUnique = true)]
public partial class EndItem
{
    [Key]
    public int Id { get; set; }

    [StringLength(45)]
    public string? InternalEndItem { get; set; }

    [StringLength(50)]
    public string? Type { get; set; }

    [StringLength(150)]
    public string? MtcCriteria { get; set; }

    public int ModelId { get; set; }

    [ForeignKey("ModelId")]
    [InverseProperty("EndItems")]
    public virtual Model Model { get; set; } = null!;

    [InverseProperty("EndItem")]
    public virtual ICollection<ModVar> ModelVariants { get; set; } = [];

    [InverseProperty("EndItem")]
    public virtual ICollection<MtcForEndItem> MtcForEndItems { get; set; } = [];
}