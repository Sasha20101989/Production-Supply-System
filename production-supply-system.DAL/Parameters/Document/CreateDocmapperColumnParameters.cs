using DAL.Models.Document;

namespace DAL.Parameters.Document
{
    public class CreateDocmapperParameters
    {
        public CreateDocmapperParameters(Docmapper entity)
        {
            DocmapperName = entity.DocmapperName;

            DefaultFolder = entity.DefaultFolder;

            SheetName = entity.SheetName;

            FirstDataRow = entity.FirstDataRow;
        }

        public string DocmapperName { get; set; }

        public string? DefaultFolder { get; set; }

        public string SheetName { get; set; } = null!;

        public int FirstDataRow { get; set; }
    }
}