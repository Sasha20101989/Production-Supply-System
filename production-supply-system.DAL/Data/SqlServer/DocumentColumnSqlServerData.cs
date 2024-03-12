using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models.Document;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class DocumentColumnSqlServerData : SqlServerData<DocmapperColumn>
    {
        public DocumentColumnSqlServerData(ISqlDataAccess db, ILogger<DocumentColumnSqlServerData> logger)
           : base(db, logger, StoredProcedureDocmapper.GetAllDocmapperColumns)
        { }
    }
}
