using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.BomContext.Models;

[Table("tbd_PartsType")]
public partial class PartsType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(20)]
    public string? PartType { get; set; }

    [InverseProperty("PartType")]
    public virtual ICollection<Part> Parts { get; set; } = [];
}