using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.BomContext.Models;

[Table("tbd_MtcGroups")]
public partial class MtcGroup
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(15)]
    public string? CriteriaGroup { get; set; }

    [InverseProperty("MtcGroup")]
    public virtual ICollection<MtcCriterion> MtcCriteria { get; set; } = [];
}