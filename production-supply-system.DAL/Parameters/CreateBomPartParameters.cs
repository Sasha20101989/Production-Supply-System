using DAL.Models.BOM;

namespace DAL.Parameters
{
    public class CreateBomPartParameters
    {
        public CreateBomPartParameters(BomPart entity)
        {
            PartNumber = entity.PartNumber;

            PartName = entity.PartName;
        }

        public string PartNumber { get; set; }

        public string? PartName { get; set; }
    }
}
