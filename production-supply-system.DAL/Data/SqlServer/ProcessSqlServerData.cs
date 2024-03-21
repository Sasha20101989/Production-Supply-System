using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Models.Master;
using DAL.Enums;

namespace DAL.Data.SqlServer
{
    public class ProcessSqlServerData(ISqlDataAccess db) : SqlServerData<Process>(db, StoredProcedureMaster.GetAllProcesses)
    {
    }
}
