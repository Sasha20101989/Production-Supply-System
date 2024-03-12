using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    /// <summary>
    /// Реализация SQL Server для получения отправителей.
    /// </summary>
    public class ShipperSqlServerData : SqlServerData<Shipper>
    {
        public ShipperSqlServerData(ISqlDataAccess db, ILogger<ShipperSqlServerData> logger)
                    : base(db, logger, StoredProcedureInbound.GetAllShippers)
        { }
    }
}
