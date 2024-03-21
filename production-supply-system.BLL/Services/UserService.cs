using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Contracts;
using BLL.Properties;

using DAL.Data.Repositories.Contracts;
using DAL.Models;

using DocumentFormat.OpenXml.Spreadsheet;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using production_supply_system.EntityFramework.DAL.Context;

namespace BLL.Services
{
    /// <summary>
    /// Сервис, отвечающий за операции, связанные с пользователями.
    /// </summary>
    /// <remarks>
    /// Инициализирует новый экземпляр класса <see cref="UserService"/>.
    /// </remarks>
    /// <param name="userRepository">Репозиторий для доступа к информации о пользователях.</param>
    public class UserService(IRepository<User> userRepository,
        IStaticDataService staticDataService,
        ILogger<UserService> logger) : IUserService
    {
        private User _user;

        /// <inheritdoc />
        public async Task<User> GetUserInfoAsync(string userAccount)
        {
            IEnumerable<User> users;

            try
            {
                using(PSSContext db = new())
                {
                    production_supply_system.EntityFramework.DAL.Models.UsersSchema.User user = await db.Users.FirstOrDefaultAsync(u => u.Account == userAccount);
                };

                logger.LogInformation($"{Resources.LogUsersGet}");

                users = await userRepository.GetAllAsync();

                logger.LogInformation($"{Resources.LogUsersGet} {Resources.Completed}");
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogUsersGet}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }

            logger.LogInformation($"{string.Format(Resources.LogUsersGet, userAccount)}");

            _user = users.SingleOrDefault(u => u.Account == userAccount);

            logger.LogInformation($"{string.Format(Resources.LogUsersGet, userAccount)} {Resources.Completed}");

            if (_user is null)
            {
                logger.LogError($"{string.Format(Resources.LogUsersNotFoundWithAccount, userAccount)}");

                return null;
            }

            _user.Section = await staticDataService.GetSectionByIdAsync(_user.SectionId);

            return _user;
        }

        /// <inheritdoc />
        public User GetCurrentUser()
        {
            return _user is null ? throw new Exception(Resources.LogUsersNotFound) : _user;
        }

        /// <inheritdoc />
        public async Task<bool> IsAccessAllowedAsync()
        {
            return await userRepository.TestConnectionAsync();
        }
    }
}
