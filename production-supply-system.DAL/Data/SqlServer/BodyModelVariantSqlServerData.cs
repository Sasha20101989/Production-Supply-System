using DAL.DbAccess.Contracts;
using DAL.Models;
using Microsoft.Extensions.Logging;
using DAL.Enums;

namespace DAL.Data.SqlServer
{
    public class BodyModelVariantSqlServerData : SqlServerData<BodyModelVariant>
    {
        public BodyModelVariantSqlServerData(ISqlDataAccess db, ILogger<BodyModelVariantSqlServerData> logger)
             : base(db, logger, StoredProcedureDbo.GetAllModelVariants)
        { }
    }
}
