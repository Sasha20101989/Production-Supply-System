using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Enums;
using DAL.Models;
using DAL.Models.Inbound;

namespace DAL.Data.SqlServer
{
    public class TracingSqlServerData(ISqlDataAccess db) : SqlServerData<Tracing>(db, StoredProcedureInbound.GetAllTracing)
    {
    }
}
