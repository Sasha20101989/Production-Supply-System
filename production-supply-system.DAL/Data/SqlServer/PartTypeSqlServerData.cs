using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Enums;
using DAL.Models;

namespace DAL.Data.SqlServer
{
    public class PartTypeSqlServerData : SqlServerData<TypesOfPart>
    {
        public PartTypeSqlServerData(ISqlDataAccess db, ILogger<PartTypeSqlServerData> logger)
            : base(db, logger, StoredProcedureInbound.GetAllPartTypes)
        { }
    }
}
