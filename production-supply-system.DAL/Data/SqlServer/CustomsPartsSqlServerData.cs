using DAL.DbAccess.Contracts;
using DAL.Models;
using Microsoft.Extensions.Logging;
using DAL.Enums;

namespace DAL.Data.SqlServer
{
    /// <summary>
    /// Реализация SQL Server для получения перевозчиков.
    /// </summary>
    public class CustomsPartsSqlServerData : SqlServerData<CustomsPart>
    {
        public CustomsPartsSqlServerData(ISqlDataAccess db, ILogger<CustomsPartsSqlServerData> logger)
             : base(db, logger, StoredProcedureCustoms.GetAllCustomsParts)
        { }
    }
}
