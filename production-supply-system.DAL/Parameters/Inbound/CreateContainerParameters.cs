using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreateContainerParameters
    {
        public CreateContainerParameters(ContainersInLot entity)
        {
            LotId = entity.LotId;
            ContainerNumber = entity.ContainerNumber;
            SealNumber = entity.SealNumber;
            ContainerTypeId = entity.ContainerTypeId;
        }

        public int LotId { get; set; }

        public string ContainerNumber { get; set; }

        public string? SealNumber { get; set; }

        public int ContainerTypeId { get; set; }
    }
}
