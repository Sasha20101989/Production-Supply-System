using DAL.Data.Contracts;
using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Data.SqlServer
{
    /// <summary>
    /// Реализация SQL Server для получения пользовательских данных.
    /// </summary>
    public class UserSqlServerData : IUserData
    {
        private readonly ISqlDataAccess _db;

        private readonly ILogger<UserSqlServerData> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="UserSqlServerData"/> class.
        /// </summary>
        /// <param name="db">Служба доступа к данным SQL.</param>
        /// <param name="logger">Регистратор для отслеживания информации и ошибок.</param>
        public UserSqlServerData(ISqlDataAccess db, ILogger<UserSqlServerData> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<User> GetUserAsync(string userAccount)
        {
            try
            {
                _logger.LogInformation($"Get user for account {userAccount}");

                IEnumerable<User> result = await _db.LoadData<User>(
                    StoredProcedureUsers.GetUserByAccount,
                    new { Account = userAccount });

                _logger.LogInformation($"Successful obtaining user for account {userAccount}");

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting user for account {userAccount}: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }
    }
}
