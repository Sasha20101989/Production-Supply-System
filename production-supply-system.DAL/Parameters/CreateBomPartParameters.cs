using DAL.Models.BOM;

namespace DAL.Parameters
{
    public class CreateBomPartParameters(BomPart entity)
    {
        public string PartNumber { get; set; } = entity.PartNumber;

        public string? PartName { get; set; } = entity.PartName;
    }
}
