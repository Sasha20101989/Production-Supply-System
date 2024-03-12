using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class SectionSqlServerData : SqlServerData<Section>
    {
        public SectionSqlServerData(ISqlDataAccess db, ILogger<SectionSqlServerData> logger)
            : base(db, logger, StoredProcedureDbo.GetAllSections)
        { }
    }
}
