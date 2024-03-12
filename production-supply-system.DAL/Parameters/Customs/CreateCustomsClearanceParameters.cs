using DAL.Models;

namespace DAL.Parameters.Customs
{
    public class CreateCustomsClearanceParameters
    {
        public CreateCustomsClearanceParameters(CustomsClearance entity)
        {
            ContainerInLotId = entity.ContainersInLot.Id;
            InvoceNumber = entity.InvoceNumber;
            PartTypeId = entity.PartType.Id;
        }

        public int ContainerInLotId { get; set; }

        public string InvoceNumber { get; set; }

        public int? PartTypeId { get; set; }
    }
}
