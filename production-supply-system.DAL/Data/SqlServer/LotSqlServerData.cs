using DAL.DbAccess.Contracts;
using DAL.Models;
using Microsoft.Extensions.Logging;
using DAL.Enums;

namespace DAL.Data.SqlServer
{
    /// <summary>
    /// Реализация SQL Server для получения перевозчиков.
    /// </summary>
    public class LotSqlServerData : SqlServerData<Lot>
    {
        public LotSqlServerData(ISqlDataAccess db, ILogger<LotSqlServerData> logger)
             : base(db, logger, StoredProcedureInbound.GetAllLotItems)
        { }
    }
}
