using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace production_supply_system.EntityFramework.DAL.Models.InboundSchema;

[Table("tbd_Terms_Of_Container_Use", Schema = "Inbound")]
public partial class TermsOfContainerUse
{
    [Key]
    public int Id { get; set; }

    [Column("Init_Date")]
    public DateOnly InitDate { get; set; }

    [Column("Carrier_Id")]
    public int CarrierId { get; set; }

    public int? Detention { get; set; }

    public int? Storage { get; set; }

    [ForeignKey("CarrierId")]
    public virtual Carrier Carrier { get; set; } = null!;
}