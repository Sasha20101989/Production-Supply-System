using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class LocationSqlServerData(ISqlDataAccess db) : SqlServerData<Location>(db, StoredProcedureInbound.GetAllLocations)
    {
    }
}
