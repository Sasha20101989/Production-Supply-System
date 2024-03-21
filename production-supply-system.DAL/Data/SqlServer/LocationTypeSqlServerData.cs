using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class LocationTypeSqlServerData(ISqlDataAccess db) : SqlServerData<TypesOfLocation>(db, StoredProcedureInbound.GetAllLocationTypes)
    {
    }
}
