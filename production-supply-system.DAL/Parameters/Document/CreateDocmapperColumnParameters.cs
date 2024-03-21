using DAL.Models.Document;

namespace DAL.Parameters.Document
{
    public class CreateDocmapperParameters(Docmapper entity)
    {
        public string DocmapperName { get; set; } = entity.DocmapperName;

        public string? DefaultFolder { get; set; } = entity.DefaultFolder;

        public string SheetName { get; set; } = entity.SheetName;

        public int FirstDataRow { get; set; } = entity.FirstDataRow;
    }
}