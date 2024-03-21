using System;
using DAL.Models.Inbound;

namespace DAL.Parameters.Inbound
{
    public class CreateTraceParameters(Tracing entity)
    {
        public int ContainerInLotId { get; set; } = entity.ContainerInLotId;

        public int? CarrierId { get; set; } = entity.CarrierId;

        public int? TraceTransportId { get; set; } = entity.TraceTransportId;

        public int? TransportationTypeId { get; set; } = entity.TransportationTypeId;

        public int TraceLocationId { get; set; } = entity.TraceLocationId;

        public string? TraceTransportDocument { get; set; } = entity.TraceTransportDocument;

        public DateTime? TraceEta { get; set; } = entity.TraceEta;

        public DateTime? TraceAta { get; set; } = entity.TraceAta;

        public DateTime? TraceEtd { get; set; } = entity.TraceEtd;

        public DateTime? TraceAtd { get; set; } = entity.TraceAtd;
    }
}
