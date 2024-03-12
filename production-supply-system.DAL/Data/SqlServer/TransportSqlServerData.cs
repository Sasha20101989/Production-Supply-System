using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class TransportSqlServerData : SqlServerData<Transport>
    {
        public TransportSqlServerData(ISqlDataAccess db, ILogger<TransportSqlServerData> logger)
            : base(db, logger, StoredProcedureInbound.GetAllTransportItems)
        { }
    }
}
