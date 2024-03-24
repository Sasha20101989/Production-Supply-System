using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Types_Of_Location", Schema = "Inbound")]
public partial class TypesOfLocation
{
    [Key]
    [Column("Location_Type_Id")]
    public int LocationTypeId { get; set; }

    [Column("Location_Type")]
    [MaxLength(20, ErrorMessage = "Location Type must not exceed 20 characters.")]
    public string? LocationType { get; set; }
}