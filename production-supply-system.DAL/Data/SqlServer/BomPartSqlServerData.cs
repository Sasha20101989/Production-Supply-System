using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Enums;
using DAL.Models.BOM;

namespace DAL.Data.SqlServer
{
    public class BomPartSqlServerData(ISqlDataAccess db) : SqlServerData<BomPart>(db, StoredProcedureDbo.GetAllBomParts)
    {
    }
}
