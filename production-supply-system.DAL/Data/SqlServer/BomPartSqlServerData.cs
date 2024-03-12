using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Enums;
using DAL.Models.BOM;

namespace DAL.Data.SqlServer
{
    public class BomPartSqlServerData : SqlServerData<BomPart>
    {
        public BomPartSqlServerData(ISqlDataAccess db, ILogger<BomPartSqlServerData> logger)
             : base(db, logger, StoredProcedureDbo.GetAllBomParts)
        { }
    }
}
