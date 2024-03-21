using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models.Planning;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    /// <summary>
    /// Реализация SQL Server для получения условий поставки.
    /// </summary>
    public class VinContainerSqlServerData(ISqlDataAccess db) : SqlServerData<VinsInContainer>(db, StoredProcedurePlanning.GetAllVinContainers)
    {
    }
}
