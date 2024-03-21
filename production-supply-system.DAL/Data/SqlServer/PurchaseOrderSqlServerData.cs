using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class PurchaseOrderSqlServerData(ISqlDataAccess db) : SqlServerData<PurchaseOrder>(db, StoredProcedurePartscontrol.GetAllPurchaseOrders)
    {
    }
}
