using System;
using DAL.Models.Inbound;

namespace DAL.Parameters.Inbound
{
    public class CreateTraceParameters
    {
        public CreateTraceParameters(Tracing entity)
        {
            ContainerInLotId = entity.ContainerInLotId;
            CarrierId = entity.CarrierId;
            TraceTransportId = entity.TraceTransportId;
            TransportationTypeId = entity.TransportationTypeId;
            TraceLocationId = entity.TraceLocationId;
            TraceTransportDocument = entity.TraceTransportDocument;
            TraceEta = entity.TraceEta;
            TraceAta = entity.TraceAta;
            TraceEtd = entity.TraceEtd;
            TraceAtd = entity.TraceAtd;
        }

        public int ContainerInLotId { get; set; }

        public int? CarrierId { get; set; }

        public int? TraceTransportId { get; set; }

        public int? TransportationTypeId { get; set; }

        public int TraceLocationId { get; set; }

        public string? TraceTransportDocument { get; set; }

        public DateTime? TraceEta { get; set; }

        public DateTime? TraceAta { get; set; }

        public DateTime? TraceEtd { get; set; }

        public DateTime? TraceAtd { get; set; }
    }
}
