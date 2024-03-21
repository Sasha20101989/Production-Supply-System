using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models.Document;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class DocumentColumnSqlServerData(ISqlDataAccess db) : SqlServerData<DocmapperColumn>(db, StoredProcedureDocmapper.GetAllDocmapperColumns)
    {
    }
}
