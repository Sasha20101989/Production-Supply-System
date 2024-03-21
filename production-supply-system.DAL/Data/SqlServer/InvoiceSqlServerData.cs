using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class InvoiceSqlServerData(ISqlDataAccess db) : SqlServerData<Invoice>(db, StoredProcedureInbound.GetAllInvoiceItems)
    {
    }
}
