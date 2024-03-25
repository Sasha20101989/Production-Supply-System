using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.BomContext.Models;

[Table("tbd_Models")]
public partial class Model
{
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    public string? ModelCode { get; set; }

    [StringLength(50)]
    public string? Comment { get; set; }

    [InverseProperty("Model")]
    public virtual ICollection<EndItem> EndItems { get; set; } = [];

    [InverseProperty("Model")]
    public virtual ICollection<PartsApplication> PartsApplications { get; set; } = [];
}