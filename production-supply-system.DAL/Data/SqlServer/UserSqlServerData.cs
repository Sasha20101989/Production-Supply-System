using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class UserSqlServerData(ISqlDataAccess db) : SqlServerData<User>(db, StoredProcedureUsers.GetAllUsers)
    {
    }
}
