using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using production_supply_system.EntityFramework.DAL.Enums;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Types_Of_Part", Schema = "Inbound")]
public partial class TypesOfPart
{
    [Key]
    [Column("Part_Type_Id")]
    public int PartTypeId { get; set; }

    [Column("Part_Type")]
    [StringLength(10)]
    public PartTypes PartType { get; set; }

    [InverseProperty("PartType")]
    public virtual ICollection<CustomsClearance> CustomsClearances { get; set; } = new List<CustomsClearance>();

    [InverseProperty("PartType")]
    public virtual ICollection<CustomsPart> CustomsParts { get; set; } = new List<CustomsPart>();
}