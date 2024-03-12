using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Models.Master;
using DAL.Enums;

namespace DAL.Data.SqlServer
{
    public class ProcessStepSqlServerData : SqlServerData<ProcessStep>
    {
        public ProcessStepSqlServerData(ISqlDataAccess db, ILogger<ProcessStepSqlServerData> logger)
            : base(db, logger, StoredProcedureMaster.GetAllProcessSteps)
        { }
    }
}
