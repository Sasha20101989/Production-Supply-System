using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class LocationTypeSqlServerData : SqlServerData<TypesOfLocation>
    {
        public LocationTypeSqlServerData(ISqlDataAccess db, ILogger<LocationTypeSqlServerData> logger)
            : base(db, logger, StoredProcedureInbound.GetAllLocationTypes)
        { }
    }
}
