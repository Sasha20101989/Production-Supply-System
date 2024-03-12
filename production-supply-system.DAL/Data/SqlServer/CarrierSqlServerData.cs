using DAL.DbAccess.Contracts;
using DAL.Models;
using Microsoft.Extensions.Logging;
using DAL.Enums;

namespace DAL.Data.SqlServer
{
    /// <summary>
    /// Реализация SQL Server для получения перевозчиков.
    /// </summary>
    public class CarrierSqlServerData : SqlServerData<Carrier>
    {
        public CarrierSqlServerData(ISqlDataAccess db, ILogger<CarrierSqlServerData> logger)
             : base(db, logger, StoredProcedureInbound.GetAllCarriers)
        { }
    }
}
