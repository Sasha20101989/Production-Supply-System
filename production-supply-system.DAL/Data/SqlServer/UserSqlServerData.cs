using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Data.SqlServer
{
    public class UserSqlServerData : SqlServerData<User>
    {
        public UserSqlServerData(ISqlDataAccess db, ILogger<UserSqlServerData> logger)
            : base(db, logger, StoredProcedureUsers.GetAllUsers)
        { }
    }
}
