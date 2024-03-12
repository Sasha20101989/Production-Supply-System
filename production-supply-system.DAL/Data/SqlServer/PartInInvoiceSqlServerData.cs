using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Enums;
using DAL.Models;

namespace DAL.Data.SqlServer
{
    public class PartInInvoiceSqlServerData : SqlServerData<PartsInInvoice>
    {
        public PartInInvoiceSqlServerData(ISqlDataAccess db, ILogger<PartInInvoiceSqlServerData> logger)
            : base(db, logger, StoredProcedureInbound.GetAllPartsInInvoice)
        { }
    }
}
 