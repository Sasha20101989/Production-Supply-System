using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class ContainerSqlServerData : SqlServerData<ContainersInLot>
    {
        public ContainerSqlServerData(ISqlDataAccess db, ILogger<ContainerSqlServerData> logger)
            : base(db, logger, StoredProcedureInbound.GetAllContainers)
        { }
    }
}
