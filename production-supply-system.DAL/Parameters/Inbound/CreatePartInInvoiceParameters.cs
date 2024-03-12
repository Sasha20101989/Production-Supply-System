using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreatePartInInvoiceParameters
    {
        public CreatePartInInvoiceParameters(PartsInInvoice entity)
        {
            InvoiceId = entity.InvoiceId;
            PartNumberId = entity.PartNumberId;
            Quantity = entity.Quantity;
            Price = entity.Price;
        }

        public int InvoiceId { get; set; }

        public int PartNumberId { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
