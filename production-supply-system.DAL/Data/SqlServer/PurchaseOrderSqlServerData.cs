using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class PurchaseOrderSqlServerData : SqlServerData<PurchaseOrder>
    {
        public PurchaseOrderSqlServerData(ISqlDataAccess db, ILogger<PurchaseOrderSqlServerData> logger)
            : base(db, logger, StoredProcedurePartscontrol.GetAllPurchaseOrders)
        { }
    }
}
