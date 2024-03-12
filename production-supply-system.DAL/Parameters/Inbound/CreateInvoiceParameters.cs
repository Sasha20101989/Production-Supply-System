using System;
using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreateInvoiceParameters
    {
        public CreateInvoiceParameters(Invoice entity)
        {
            InvoiceNumber = entity.InvoiceNumber;
            InvoiceDate = entity.InvoiceDate;
            ShipperId = entity.ShipperId;
            PurchaseOrderId = entity.PurchaseOrderId;
        }

        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }

        public int ShipperId { get; set; }

        public int? PurchaseOrderId { get; set; } = null!;
    }
}
