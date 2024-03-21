using System;
using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreateInvoiceParameters(Invoice entity)
    {
        public string InvoiceNumber { get; set; } = entity.InvoiceNumber;
        public DateTime InvoiceDate { get; set; } = entity.InvoiceDate;

        public int ShipperId { get; set; } = entity.ShipperId;

        public int? PurchaseOrderId { get; set; } = entity.PurchaseOrderId;
    }
}
