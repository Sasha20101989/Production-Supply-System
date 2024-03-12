using DAL.Models.Planning;

namespace DAL.Parameters.Planning
{
    public class CreateVinContainerParameters
    {
        public CreateVinContainerParameters(VinsInContainer entity)
        {
            ContainerInLotId = entity.ContainerInLotId;
            SupplierVinNumber = entity.SupplierVinNumber;
            ModvarId = entity.ModvarId;
            LotId = entity.LotId;
        }

        public int ContainerInLotId { get; set; }

        public string SupplierVinNumber { get; set; }

        public int ModvarId { get; set; }

        public int LotId { get; set; }
    }
}
