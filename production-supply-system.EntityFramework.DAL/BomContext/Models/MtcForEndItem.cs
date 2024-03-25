using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.BomContext.Models;

[Table("tbd_MtcForEndItems")]
public partial class MtcForEndItem
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("EndItemID")]
    public int EndItemId { get; set; }

    [Column("MtcCriteriaID")]
    public int MtcCriteriaId { get; set; }

    [ForeignKey("EndItemId")]
    [InverseProperty("MtcForEndItems")]
    public virtual EndItem EndItem { get; set; } = null!;

    [ForeignKey("MtcCriteriaId")]
    [InverseProperty("MtcForEndItems")]
    public virtual MtcCriterion MtcCriteria { get; set; } = null!;
}