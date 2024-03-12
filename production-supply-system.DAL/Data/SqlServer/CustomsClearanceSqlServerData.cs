using DAL.DbAccess.Contracts;
using DAL.Models;
using Microsoft.Extensions.Logging;
using DAL.Enums;

namespace DAL.Data.SqlServer
{
    /// <summary>
    /// Реализация SQL Server для получения информации о таможенных процедурах.
    /// </summary>
    public class CustomsClearanceSqlServerData : SqlServerData<CustomsClearance>
    {
        public CustomsClearanceSqlServerData(ISqlDataAccess db, ILogger<CustomsClearanceSqlServerData> logger)
             : base(db, logger, StoredProcedureCustoms.GetAllCustomsClearance)
        { }
    }
}
