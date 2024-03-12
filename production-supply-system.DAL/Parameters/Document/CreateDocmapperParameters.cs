using DAL.Models.Document;

namespace DAL.Parameters.Document
{
    public class CreateDocmapperColumnParameters
    {
        public CreateDocmapperColumnParameters(DocmapperColumn entity)
        {
            ElementName = entity.ElementName;

            SystemColumnName = entity.SystemColumnName;
        }

        public string ElementName { get; set; }

        public string SystemColumnName { get; set; }    
    }
}