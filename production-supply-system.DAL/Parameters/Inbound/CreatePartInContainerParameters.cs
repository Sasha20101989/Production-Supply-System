using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreatePartInContainerParameters(PartsInContainer entity)
    {
        public int? CaseId { get; set; } = entity.Case.Id;

        public int ContainerInLotId { get; set; } = entity.ContainerInLot.Id;

        public int PartInvoiceId { get; set; } = entity.PartInvoice.Id;

        public int PartNumberId { get; set; } = entity.PartNumber.PartNumberId;

        public decimal Quantity { get; set; } = entity.Quantity;
    }
}
