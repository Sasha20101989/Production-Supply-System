using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class TransportTypeSqlServerData : SqlServerData<TypesOfTransport>
    {
        public TransportTypeSqlServerData(ISqlDataAccess db, ILogger<TransportTypeSqlServerData> logger)
            : base(db, logger, StoredProcedureInbound.GetAllTransportTypes)
        { }
    }
}
