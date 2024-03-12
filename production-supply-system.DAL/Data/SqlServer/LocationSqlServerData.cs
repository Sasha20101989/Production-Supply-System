using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class LocationSqlServerData : SqlServerData<Location>
    {
        public LocationSqlServerData(ISqlDataAccess db, ILogger<LocationSqlServerData> logger)
            : base(db, logger, StoredProcedureInbound.GetAllLocations)
        { }
    }
}
