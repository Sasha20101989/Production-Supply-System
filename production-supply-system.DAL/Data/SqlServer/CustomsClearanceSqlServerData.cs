using DAL.DbAccess.Contracts;
using DAL.Models;
using Microsoft.Extensions.Logging;
using DAL.Enums;

namespace DAL.Data.SqlServer
{
    /// <summary>
    /// Реализация SQL Server для получения информации о таможенных процедурах.
    /// </summary>
    public class CustomsClearanceSqlServerData(ISqlDataAccess db) : SqlServerData<CustomsClearance>(db, StoredProcedureCustoms.GetAllCustomsClearance)
    {
    }
}
