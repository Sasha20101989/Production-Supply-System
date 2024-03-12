using DAL.Models.Document;

namespace DAL.Parameters.Document
{
    public class UpdateDocmapperParameters
    {
        public UpdateDocmapperParameters(Docmapper entity)
        {
            Id = entity.Id;

            DocmapperName = entity.DocmapperName;

            DefaultFolder = entity.DefaultFolder;

            SheetName = entity.SheetName;

            FirstDataRow = entity.FirstDataRow;

            IsActive = entity.IsActive;
        }
        public int Id { get; set; }

        public string DocmapperName { get; set; }

        public string? DefaultFolder { get; set; } = null!;

        public string SheetName { get; set; }

        public int FirstDataRow { get; set; }

        public bool IsActive { get; set; }
    }
}
