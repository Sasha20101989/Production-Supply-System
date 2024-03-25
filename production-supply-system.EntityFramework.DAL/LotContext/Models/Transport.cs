using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Transports", Schema = "Inbound")]
[Index("TransportName", Name = "IX_tbd_Transports", IsUnique = true)]
public partial class Transport
{
    [Key]
    [Column("Transport_Id")]
    public int Id { get; set; }

    [Column("Transport_Name")]
    [StringLength(50)]
    public string? TransportName { get; set; }
}