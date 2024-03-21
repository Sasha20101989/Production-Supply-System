using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class SectionSqlServerData(ISqlDataAccess db) : SqlServerData<Section>(db, StoredProcedureDbo.GetAllSections)
    {
    }
}
