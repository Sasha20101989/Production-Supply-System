using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class TransportTypeSqlServerData(ISqlDataAccess db) : SqlServerData<TypesOfTransport>(db, StoredProcedureInbound.GetAllTransportTypes)
    {
    }
}
