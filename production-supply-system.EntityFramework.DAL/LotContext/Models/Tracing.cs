using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Tracing", Schema = "Inbound")]
public partial class Tracing
{
    [Key]
    [Column("Tracing_Id")]
    public int TracingId { get; set; }

    [Column("Container_In_Lot_Id")]
    public int ContainerInLotId { get; set; }

    [Column("Carrier_Id")]
    public int? CarrierId { get; set; }

    [Column("Trace_Transport_Id")]
    public int? TraceTransportId { get; set; }

    [Column("Transportation_Type_Id")]
    public int? TransportationTypeId { get; set; }

    [Column("Trace_Location_Id")]
    public int TraceLocationId { get; set; }

    [Column("Trace_Transport_Document")]
    [StringLength(50)]
    public string? TraceTransportDocument { get; set; }

    [Column("Trace_ETA")]
    public DateTime? TraceEta { get; set; }

    [Column("Trace_ATA")]
    public DateTime? TraceAta { get; set; }

    [Column("Trace_ETD")]
    public DateTime? TraceEtd { get; set; }

    [Column("Trace_ATD")]
    public DateTime? TraceAtd { get; set; }

    [ForeignKey("CarrierId")]
    public virtual Carrier? Carrier { get; set; }

    [ForeignKey("ContainerInLotId")]
    public virtual ContainersInLot ContainerInLot { get; set; } = null!;

    [ForeignKey("TraceLocationId")]
    public virtual Location TraceLocation { get; set; } = null!;

    [ForeignKey("TraceTransportId")]
    public virtual Transport? TraceTransport { get; set; }

    [ForeignKey("TransportationTypeId")]
    public virtual TypesOfTransport? TransportationType { get; set; }
}