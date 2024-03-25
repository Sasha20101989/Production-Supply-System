using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Carriers", Schema = "Inbound")]
public partial class Carrier
{
    [Key]
    [Column("Carrier_Id")]
    public int Id { get; set; }

    [Column("Carrier_Name")]
    [StringLength(50)]
    public string? CarrierName { get; set; }
}