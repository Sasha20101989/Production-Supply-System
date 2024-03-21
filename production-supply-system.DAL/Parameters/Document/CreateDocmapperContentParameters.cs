using DAL.Models.Document;

namespace DAL.Parameters.Document
{
    public class UpdateDocmapperContentParameters(DocmapperContent entity)
    {
        public int Id { get; set; } = entity.Id;

        public int? RowNumber { get; set; } = entity.RowNr;

        public int ColumnNumber { get; set; } = entity.ColumnNr;
    }
}
