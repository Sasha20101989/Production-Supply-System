using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models.Partscontrol;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class PurchaseOrderTypeSqlServerData : SqlServerData<TypesOfOrder>
    {
        public PurchaseOrderTypeSqlServerData(ISqlDataAccess db, ILogger<PurchaseOrderTypeSqlServerData> logger)
            : base(db, logger, StoredProcedurePartscontrol.GetAllPurchaseOrderTypes)
        { }
    }
}
