using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    /// <summary>
    /// Реализация SQL Server для получения отправителей.
    /// </summary>
    public class ShipperSqlServerData(ISqlDataAccess db) : SqlServerData<Shipper>(db, StoredProcedureInbound.GetAllShippers)
    {
    }
}
