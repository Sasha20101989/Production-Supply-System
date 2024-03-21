using DAL.DbAccess.Contracts;
using DAL.Models;
using Microsoft.Extensions.Logging;
using DAL.Enums;

namespace DAL.Data.SqlServer
{
    /// <summary>
    /// Реализация SQL Server для получения перевозчиков.
    /// </summary>
    public class CaseSqlServerData(ISqlDataAccess db) : SqlServerData<Case>(db, StoredProcedureInbound.GetAllCases)
    {
    }
}
