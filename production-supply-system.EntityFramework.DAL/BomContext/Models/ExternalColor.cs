using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.BomContext.Models;

[Table("tbd_ExternalColors")]
public partial class ExternalColor
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(3)]
    public string? Code { get; set; }

    [StringLength(15)]
    public string? NameRus { get; set; }

    [StringLength(15)]
    public string? NameEng { get; set; }

    [InverseProperty("ExtCol")]
    public virtual ICollection<ModVar> ModelVariants { get; set; } = [];

    [InverseProperty("ExtColorNavigation")]
    public virtual ICollection<Part> Parts { get; set; } = [];
}