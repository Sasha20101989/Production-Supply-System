using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class ContainerSqlServerData(ISqlDataAccess db) : SqlServerData<ContainersInLot>(db, StoredProcedureInbound.GetAllContainers)
    {
    }
}
