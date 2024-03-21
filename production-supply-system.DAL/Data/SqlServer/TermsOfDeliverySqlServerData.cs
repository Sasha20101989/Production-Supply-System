using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    /// <summary>
    /// Реализация SQL Server для получения условий поставки.
    /// </summary>
    public class TermsOfDeliverySqlServerData(ISqlDataAccess db) : SqlServerData<TermsOfDelivery>(db, StoredProcedureInbound.GetAllTermsOfDelivery)
    {
    }
}
