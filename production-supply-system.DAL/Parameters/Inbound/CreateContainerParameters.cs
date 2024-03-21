using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreateContainerParameters(ContainersInLot entity)
    {
        public int LotId { get; set; } = entity.LotId;

        public string ContainerNumber { get; set; } = entity.ContainerNumber;

        public string? SealNumber { get; set; } = entity.SealNumber;

        public int ContainerTypeId { get; set; } = entity.ContainerTypeId;
    }
}
