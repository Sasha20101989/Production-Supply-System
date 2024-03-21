using DAL.Models.Document;

namespace DAL.Parameters.Document
{
    public class CreateDocmapperContentParameters(DocmapperContent entity)
    {
        public int DocmapperId { get; set; } = entity.DocmapperId;

        public int DocmapperColumnId { get; set; } = entity.DocmapperColumnId;

        public int? RowNumber { get; set; } = entity.RowNr;

        public int ColumnNumber { get; set; } = entity.ColumnNr;
    }
}
