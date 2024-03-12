using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Enums;
using DAL.Models;

namespace DAL.Data.SqlServer
{
    public class PartInContainerSqlServerData : SqlServerData<PartsInContainer>
    {
        public PartInContainerSqlServerData(ISqlDataAccess db, ILogger<PartInContainerSqlServerData> logger)
            : base(db, logger, StoredProcedureInbound.GetAllPartsInContainer)
        { }
    }
}
