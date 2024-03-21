using System;
using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreateLotParameters(Lot entity)
    {
        public string LotNumber { get; set; } = entity.LotNumber;

        public int ShipperId { get; set; } = entity.ShipperId;

        public int LotInvoiceId { get; set; } = entity.LotInvoiceId;

        public int LotPurchaseOrderId { get; set; } = entity.LotPurchaseOrderId;

        public int CarrierId { get; set; } = entity.CarrierId;

        public int DeliveryTermsId { get; set; } = entity.DeliveryTermsId;

        public int? LotTransportId { get; set; } = entity.LotTransportId;

        public int LotTransportTypeId { get; set; } = entity.LotTransportTypeId;

        public string? LotTransportDocument { get; set; } = entity.LotTransportDocument;

        public DateTime? LotEtd { get; set; } = entity.LotEtd;

        public DateTime? LotAtd { get; set; } = entity.LotAtd;

        public DateTime? LotEta { get; set; } = entity.LotEta;

        public DateTime? LotAta { get; set; } = entity.LotAta;

        public int LotDepartureLocationId { get; set; } = entity.LotDepartureLocationId;

        public int? LotCustomsLocationId { get; set; } = entity.LotCustomsLocationId;

        public int LotArrivalLocationId { get; set; } = entity.LotArrivalLocationId;

        public string? LotComment { get; set; } = entity.LotComment;
    }
}
