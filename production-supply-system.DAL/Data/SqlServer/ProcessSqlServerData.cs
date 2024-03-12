using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Models.Master;
using DAL.Enums;

namespace DAL.Data.SqlServer
{
    public class ProcessSqlServerData : SqlServerData<Process>
    {
        public ProcessSqlServerData(ISqlDataAccess db, ILogger<ProcessSqlServerData> logger)
            : base(db, logger, StoredProcedureMaster.GetAllProcesses)
        { }
    }
}
