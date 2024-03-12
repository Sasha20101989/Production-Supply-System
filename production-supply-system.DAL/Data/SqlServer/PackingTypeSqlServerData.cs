using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Enums;
using DAL.Models;

namespace DAL.Data.SqlServer
{
    public class PackingTypeSqlServerData : SqlServerData<TypesOfPacking>
    {
        public PackingTypeSqlServerData(ISqlDataAccess db, ILogger<PackingTypeSqlServerData> logger)
            : base(db, logger, StoredProcedureInbound.GetAllPackingTypes)
        { }
    }
}
