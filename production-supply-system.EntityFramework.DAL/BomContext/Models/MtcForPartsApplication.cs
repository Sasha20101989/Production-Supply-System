using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.BomContext.Models;

[Table("tbd_MtcForPartsApplication")]
public partial class MtcForPartsApplication
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("PartApplicationID")]
    public int PartApplicationId { get; set; }

    [Column("MtcCriteriaID")]
    public int MtcCriteriaId { get; set; }

    [ForeignKey("MtcCriteriaId")]
    [InverseProperty("MtcForPartsApplications")]
    public virtual MtcCriterion MtcCriteria { get; set; } = null!;

    [ForeignKey("PartApplicationId")]
    [InverseProperty("MtcForPartsApplications")]
    public virtual PartsApplication PartApplication { get; set; } = null!;
}