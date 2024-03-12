using DAL.Models;

namespace DAL.Parameters.Inbound
{
    public class CreateCaseParameters
    {
        public CreateCaseParameters(Case entity)
        {
            CaseNo = entity.CaseNo;
            NetWeight = entity.NetWeight;
            GrossWeight = entity.GrossWeight;
            Length = entity.Length;
            Width = entity.Width;
            Height = entity.Height;
            Volume = entity.Volume;
            PackingTypeId = entity.PackingTypeId;
        }

        public string CaseNo { get; set; }

        public decimal NetWeight { get; set; }

        public decimal GrossWeight { get; set; }

        public decimal? Length { get; set; }

        public decimal? Width { get; set; }

        public decimal? Height { get; set; }

        public decimal? Volume { get; set; }

        public int? PackingTypeId { get; set; }
    }
}
