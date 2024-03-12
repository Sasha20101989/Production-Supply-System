using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Enums;
using DAL.Models;
using DAL.Models.Inbound;

namespace DAL.Data.SqlServer
{
    public class TracingSqlServerData : SqlServerData<Tracing>
    {
        public TracingSqlServerData(ISqlDataAccess db, ILogger<TracingSqlServerData> logger)
            : base(db, logger, StoredProcedureInbound.GetAllTracing)
        { }
    }
}
