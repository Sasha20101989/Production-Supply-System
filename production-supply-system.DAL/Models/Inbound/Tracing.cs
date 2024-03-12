using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Contracts;
using System.Threading;

namespace DAL.Models.Inbound
{
    [Table("tbd_Tracing", Schema = "Inbound")]
    public partial class Tracing : IEntity
    {
        private ContainersInLot _containerInLot;
        private Carrier _carrier;
        private Location _traceLocation;
        private Transport _traceTransport;
        private TypesOfTransport _traceTransportType;

        [Key]
        [Column("Tracing_Id")]
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "Trace Transport Document must not exceed 50 characters.")]
        [Column("Trace_Transport_Document")]
        public string? TraceTransportDocument { get; set; }

        [Column("Trace_ETA")]
        public DateTime? TraceEta { get; set; }

        [Column("Trace_ATA")]
        public DateTime? TraceAta { get; set; }

        [Column("Trace_ETD")]
        public DateTime? TraceEtd { get; set; }

        [Column("Trace_ATD")]
        public DateTime? TraceAtd { get; set; }

        [Column("Carrier_Id")]
        public int? CarrierId { get; set; }

        [ForeignKey("CarrierId")]
        public virtual Carrier Carrier
        {
            get => _carrier;
            set
            {
                _carrier = value;
                CarrierId = value?.Id ?? null;
            }
        }

        [Column("Container_In_Lot_Id")]
        public int ContainerInLotId { get; set; }

        [ForeignKey("ContainerInLotId")]
        public virtual ContainersInLot ContainerInLot
        {
            get => _containerInLot;
            set
            {
                _containerInLot = value;
                ContainerInLotId = value?.Id ?? 0;
            }
        }

        [Column("Trace_Location_Id")]
        public int TraceLocationId { get; set; }

        [ForeignKey("TraceLocationId")]
        public virtual Location TraceLocation
        {
            get => _traceLocation;
            set
            {
                _traceLocation = value;
                TraceLocationId = value?.Id ?? 0;
            }
        }

        [Column("Trace_Transport_Id")]
        public int? TraceTransportId { get; set; }

        [ForeignKey("TraceTransportId")]
        public virtual Transport TraceTransport
        {
            get => _traceTransport;
            set
            {
                _traceTransport = value;
                TraceTransportId = value?.Id ?? null;
            }
        }

        [Column("Transportation_Type_Id")]
        public int? TransportationTypeId { get; set; }

        [ForeignKey("TransportationTypeId")]
        public virtual TypesOfTransport TransportationType
        {
            get => _traceTransportType;
            set
            {
                _traceTransportType = value;
                TransportationTypeId = value?.Id ?? null;
            }
        }
    }
}
