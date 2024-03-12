using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreatePartInContainerParameters
    {
        public CreatePartInContainerParameters(PartsInContainer entity)
        {
            CaseId = entity.Case.Id;
            ContainerInLotId = entity.ContainerInLot.Id;
            PartInvoiceId = entity.PartInvoice.Id;
            PartNumberId = entity.PartNumber.PartNumberId;
            Quantity = entity.Quantity;
        }

        public int? CaseId { get; set; }

        public int ContainerInLotId { get; set; }

        public int PartInvoiceId { get; set; }

        public int PartNumberId { get; set; }

        public decimal Quantity { get; set; }
    }
}
