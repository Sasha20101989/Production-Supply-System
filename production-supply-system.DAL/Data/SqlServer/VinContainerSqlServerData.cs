using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models.Planning;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    /// <summary>
    /// Реализация SQL Server для получения условий поставки.
    /// </summary>
    public class VinContainerSqlServerData : SqlServerData<VinsInContainer>
    {
        public VinContainerSqlServerData(ISqlDataAccess db, ILogger<VinContainerSqlServerData> logger)
                    : base(db, logger, StoredProcedurePlanning.GetAllVinContainers)
        { }
    }
}
