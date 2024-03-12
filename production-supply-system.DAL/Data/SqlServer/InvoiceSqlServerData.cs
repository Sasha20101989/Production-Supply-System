using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class InvoiceSqlServerData : SqlServerData<Invoice>
    {
        public InvoiceSqlServerData(ISqlDataAccess db, ILogger<InvoiceSqlServerData> logger)
          : base(db, logger, StoredProcedureInbound.GetAllInvoiceItems)
        { }
    }
}
