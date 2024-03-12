using System;
using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreateLotParameters
    {
        public CreateLotParameters(Lot entity)
        {
            LotNumber = entity.LotNumber;
            ShipperId = entity.ShipperId;
            LotInvoiceId = entity.LotInvoiceId;
            LotPurchaseOrderId = entity.LotPurchaseOrderId;
            CarrierId = entity.CarrierId;
            DeliveryTermsId = entity.DeliveryTermsId;
            LotTransportId = entity.LotTransportId;
            LotTransportTypeId = entity.LotTransportTypeId;
            LotTransportDocument = entity.LotTransportDocument;
            LotEtd = entity.LotEtd;
            LotAtd = entity.LotAtd;
            LotEta = entity.LotEta;
            LotAta = entity.LotAta;
            LotDepartureLocationId = entity.LotDepartureLocationId;
            LotCustomsLocationId = entity.LotCustomsLocationId;
            LotArrivalLocationId = entity.LotArrivalLocationId;
            LotComment = entity.LotComment;
        }

        public string LotNumber { get; set; }

        public int ShipperId { get; set; }

        public int LotInvoiceId { get; set; }

        public int LotPurchaseOrderId { get; set; }

        public int CarrierId { get; set; }

        public int DeliveryTermsId { get; set; }

        public int? LotTransportId { get; set; }

        public int LotTransportTypeId { get; set; }

        public string? LotTransportDocument { get; set; }

        public DateTime? LotEtd { get; set; }

        public DateTime? LotAtd { get; set; }

        public DateTime? LotEta { get; set; }

        public DateTime? LotAta { get; set; }

        public int LotDepartureLocationId { get; set; }

        public int? LotCustomsLocationId { get; set; }

        public int LotArrivalLocationId { get; set; }

        public string? LotComment { get; set; }
    }
}
