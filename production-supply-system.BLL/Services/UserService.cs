using System;
using System.Threading.Tasks;

using BLL.Contracts;
using BLL.Properties;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using production_supply_system.EntityFramework.DAL.Context;
using production_supply_system.EntityFramework.DAL.Models.UsersSchema;

namespace BLL.Services
{
    /// <summary>
    /// Сервис, отвечающий за операции, связанные с пользователями.
    /// </summary>
    /// <remarks>
    /// Инициализирует новый экземпляр класса <see cref="UserService"/>.
    /// </remarks>
    /// <param name="userRepository">Репозиторий для доступа к информации о пользователях.</param>
    public class UserService(PSSContext db, ILogger<UserService> logger) : IUserService
    {
        /// <inheritdoc />
        public async Task<User> GetCurrentUser(string userAccount)
        {
            try
            {
                logger.LogInformation($"{string.Format(Resources.LogUsersGetByAccount, userAccount)}");

                User user = await db.Users
                    .Include(u => u.Section)
                    .FirstOrDefaultAsync(u => u.Account == userAccount);

                logger.LogInformation($"{string.Format(Resources.LogUsersGetByAccount, userAccount)} {Resources.Completed}");

                return user;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogUsersGet}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }
    }
}
