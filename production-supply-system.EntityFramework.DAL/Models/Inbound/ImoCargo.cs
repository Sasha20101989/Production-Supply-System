using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.Models.InboundSchema;

[Table("tbd_Imo_Cargo", Schema = "Inbound")]
public partial class ImoCargo
{
    [Key]
    [Column("Imo_Cargo_Id")]
    public int ImoCargoId { get; set; }

    [Column("Part_Number_Id")]
    public int PartNumberId { get; set; }

    [StringLength(150)]
    public string? Comment { get; set; }
}