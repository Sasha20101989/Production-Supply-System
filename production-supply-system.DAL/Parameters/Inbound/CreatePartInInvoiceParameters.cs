using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreatePartInInvoiceParameters(PartsInInvoice entity)
    {
        public int InvoiceId { get; set; } = entity.InvoiceId;

        public int PartNumberId { get; set; } = entity.PartNumberId;

        public decimal Quantity { get; set; } = entity.Quantity;

        public decimal Price { get; set; } = entity.Price;
    }
}
