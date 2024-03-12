using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models.Document;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class DocumentSqlServerData : SqlServerData<Docmapper>
    {
        public DocumentSqlServerData(ISqlDataAccess db, ILogger<DocumentSqlServerData> logger)
           : base(db, logger, StoredProcedureDocmapper.GetAllDocmapperItems)
        { }
    }
}
