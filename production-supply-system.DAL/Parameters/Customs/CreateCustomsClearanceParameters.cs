using DAL.Models;

namespace DAL.Parameters.Customs
{
    public class CreateCustomsClearanceParameters(CustomsClearance entity)
    {
        public int ContainerInLotId { get; set; } = entity.ContainersInLot.Id;

        public string InvoceNumber { get; set; } = entity.InvoceNumber;

        public int? PartTypeId { get; set; } = entity.PartType.Id;
    }
}
