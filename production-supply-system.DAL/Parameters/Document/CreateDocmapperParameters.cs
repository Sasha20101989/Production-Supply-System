using DAL.Models.Document;

namespace DAL.Parameters.Document
{
    public class CreateDocmapperColumnParameters(DocmapperColumn entity)
    {
        public string ElementName { get; set; } = entity.ElementName;

        public string SystemColumnName { get; set; } = entity.SystemColumnName;
    }
}