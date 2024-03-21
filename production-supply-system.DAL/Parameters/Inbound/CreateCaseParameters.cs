using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreateCaseParameters(Case entity)
    {
        public string CaseNo { get; set; } = entity.CaseNo;

        public decimal NetWeight { get; set; } = entity.NetWeight;

        public decimal GrossWeight { get; set; } = entity.GrossWeight;

        public decimal? Length { get; set; } = entity.Length;

        public decimal? Width { get; set; } = entity.Width;

        public decimal? Height { get; set; } = entity.Height;

        public decimal? Volume { get; set; } = entity.Volume;

        public int? PackingTypeId { get; set; } = entity.PackingTypeId;
    }
}
