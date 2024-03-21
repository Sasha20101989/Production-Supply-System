using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Enums;
using DAL.Models;

namespace DAL.Data.SqlServer
{
    public class PartTypeSqlServerData(ISqlDataAccess db) : SqlServerData<TypesOfPart>(db, StoredProcedureInbound.GetAllPartTypes)
    {
    }
}
