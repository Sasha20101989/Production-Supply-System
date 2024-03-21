using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Enums;
using DAL.Models;

namespace DAL.Data.SqlServer
{
    public class PackingTypeSqlServerData(ISqlDataAccess db) : SqlServerData<TypesOfPacking>(db, StoredProcedureInbound.GetAllPackingTypes)
    {
    }
}
