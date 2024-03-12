using DAL.Models.Document;

namespace DAL.Parameters.Document
{
    public class UpdateDocmapperContentParameters
    {
        public UpdateDocmapperContentParameters(DocmapperContent entity)
        {
            Id = entity.Id;

            RowNumber = entity.RowNr;

            ColumnNumber = entity.ColumnNr;
        }

        public int Id { get; set; }

        public int? RowNumber { get; set; }

        public int ColumnNumber { get; set; }
    }
}
