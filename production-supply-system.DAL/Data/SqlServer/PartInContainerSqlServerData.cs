using DAL.DbAccess.Contracts;
using Microsoft.Extensions.Logging;
using DAL.Enums;
using DAL.Models;

namespace DAL.Data.SqlServer
{
    public class PartInContainerSqlServerData(ISqlDataAccess db) : SqlServerData<PartsInContainer>(db, StoredProcedureInbound.GetAllPartsInContainer)
    {
    }
}
