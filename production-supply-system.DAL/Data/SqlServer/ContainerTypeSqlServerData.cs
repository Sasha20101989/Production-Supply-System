using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class ContainerTypeSqlServerData : SqlServerData<TypesOfContainer>
    {
        public ContainerTypeSqlServerData(ISqlDataAccess db, ILogger<ContainerTypeSqlServerData> logger)
            : base(db, logger, StoredProcedureInbound.GetAllContainerTypes)
        { }
    }
}
