using DAL.Models.Planning;

namespace DAL.Parameters.Planning
{
    public class CreateVinContainerParameters(VinsInContainer entity)
    {
        public int ContainerInLotId { get; set; } = entity.ContainerInLotId;

        public string SupplierVinNumber { get; set; } = entity.SupplierVinNumber;

        public int ModvarId { get; set; } = entity.ModvarId;

        public int LotId { get; set; } = entity.LotId;
    }
}
