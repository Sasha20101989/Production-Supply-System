using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models.Document;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class DocumentSqlServerData(ISqlDataAccess db) : SqlServerData<Docmapper>(db, StoredProcedureDocmapper.GetAllDocmapperItems)
    {
    }
}
