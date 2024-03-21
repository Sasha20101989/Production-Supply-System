using DAL.DbAccess.Contracts;
using DAL.Models;
using Microsoft.Extensions.Logging;
using DAL.Enums;

namespace DAL.Data.SqlServer
{
    public class BodyModelVariantSqlServerData(ISqlDataAccess db) : SqlServerData<BodyModelVariant>(db, StoredProcedureDbo.GetAllModelVariants)
    {
    }
}
