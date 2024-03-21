using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class ContainerTypeSqlServerData(ISqlDataAccess db) : SqlServerData<TypesOfContainer>(db, StoredProcedureInbound.GetAllContainerTypes)
    {
    }
}
