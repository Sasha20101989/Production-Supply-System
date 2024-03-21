using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Enums;
using DAL.Models;

namespace DAL.Data.SqlServer
{
    public class PartInInvoiceSqlServerData(ISqlDataAccess db) : SqlServerData<PartsInInvoice>(db, StoredProcedureInbound.GetAllPartsInInvoice)
    {
    }
}
 