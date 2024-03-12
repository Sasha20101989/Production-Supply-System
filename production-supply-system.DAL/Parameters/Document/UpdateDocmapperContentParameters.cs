using DAL.Models.Document;

namespace DAL.Parameters.Document
{
    public class CreateDocmapperContentParameters
    {
        public CreateDocmapperContentParameters(DocmapperContent entity)
        {
            DocmapperId = entity.DocmapperId;

            DocmapperColumnId = entity.DocmapperColumnId;

            RowNumber = entity.RowNr;

            ColumnNumber = entity.ColumnNr;
        }

        public int DocmapperId { get; set; }

        public int DocmapperColumnId { get; set; }

        public int? RowNumber { get; set; }

        public int ColumnNumber { get; set; }
    }
}
