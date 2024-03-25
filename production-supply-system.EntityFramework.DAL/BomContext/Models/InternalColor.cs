using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.BomContext.Models;    

[Table("tbd_InternalColors")]
public partial class InternalColor
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(6)]
    public string? Code { get; set; }

    [StringLength(15)]
    public string? NameRus { get; set; }

    [StringLength(15)]
    public string? NameEng { get; set; }

    [InverseProperty("IntCol")]
    public virtual ICollection<ModVar> ModelVariants { get; set; } = [];

    [InverseProperty("IntColorNavigation")]
    public virtual ICollection<Part> Parts { get; set; } = [];
}