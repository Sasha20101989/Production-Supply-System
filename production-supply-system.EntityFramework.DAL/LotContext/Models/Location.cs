using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Locations", Schema = "Inbound")]
public partial class Location
{
    [Key]
    [Column("Location_Id")]
    public int LocationId { get; set; }

    [Column("Location_Type_Id")]
    public int LocationTypeId { get; set; }

    [Column("Location_Name")]
    [StringLength(50)]
    public string LocationName { get; set; } = null!;

    [StringLength(50)]
    public string? City { get; set; }

    [ForeignKey("LocationTypeId")]
    public virtual TypesOfLocation LocationType { get; set; } = null!;
}