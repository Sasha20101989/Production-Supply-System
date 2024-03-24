using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Types_Of_Container", Schema = "Inbound")]
public partial class TypesOfContainer
{
    [Key]
    [Column("Container_Type_Id")]
    public int ContainerTypeId { get; set; }

    [Column("Container_Type")]
    [StringLength(10)]
    public string? ContainerType { get; set; }
}