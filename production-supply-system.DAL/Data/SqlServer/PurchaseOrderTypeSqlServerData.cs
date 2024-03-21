using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models.Partscontrol;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class PurchaseOrderTypeSqlServerData(ISqlDataAccess db) : SqlServerData<TypesOfOrder>(db, StoredProcedurePartscontrol.GetAllPurchaseOrderTypes)
    {
    }
}
