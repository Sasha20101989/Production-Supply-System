using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models.Document;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class DocumentContentSqlServerData : SqlServerData<DocmapperContent>
    {
        public DocumentContentSqlServerData(ISqlDataAccess db, ILogger<DocumentContentSqlServerData> logger)
           : base(db, logger, StoredProcedureDocmapper.GetAllDocmapperContentItems)
        { }
    }
}
