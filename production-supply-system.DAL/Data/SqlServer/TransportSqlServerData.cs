using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class TransportSqlServerData(ISqlDataAccess db) : SqlServerData<Transport>(db, StoredProcedureInbound.GetAllTransportItems)
    {
    }
}
