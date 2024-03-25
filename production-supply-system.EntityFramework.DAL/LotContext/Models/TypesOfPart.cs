using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Types_Of_Part", Schema = "Inbound")]
public partial class TypesOfPart
{
    [Key]
    [Column("Part_Type_Id")]
    public int Id { get; set; }

    [Column("Part_Type")]
    [MaxLength(10, ErrorMessage = "Part Type must not exceed 10 characters.")]
    public string? PartType { get; set; }
}