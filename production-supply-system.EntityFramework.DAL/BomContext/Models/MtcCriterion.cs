using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace production_supply_system.EntityFramework.DAL.BomContext.Models;

[Table("tbd_MtcCriteria")]
public partial class MtcCriterion
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MtcGroupID")]
    public int MtcGroupId { get; set; }

    [StringLength(10)]
    public string CriteriaName { get; set; } = null!;

    [StringLength(50)]
    public string? CriteriaDescription { get; set; }

    [ForeignKey("MtcGroupId")]
    [InverseProperty("MtcCriteria")]
    public virtual MtcGroup MtcGroup { get; set; } = null!;

    [InverseProperty("MtcCriteria")]
    public virtual ICollection<MtcForEndItem> MtcForEndItems { get; set; } = [];

    [InverseProperty("MtcCriteria")]
    public virtual ICollection<MtcForPartsApplication> MtcForPartsApplications { get; set; } = [];
}