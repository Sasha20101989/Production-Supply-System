using System;
using System.Threading.Tasks;

using BLL.Contracts;

using Microsoft.Extensions.Logging;

using production_supply_system.EntityFramework.DAL.Models.UsersSchema;

using UI_Interface.Contracts.Services;
using UI_Interface.Helpers;

namespace UI_Interface.Services
{
    /// <summary>
    /// Сервис аутентификации пользователя.
    /// </summary>
    /// <remarks>
    /// Инициализирует новый экземпляр службы аутентификации.
    /// </remarks>
    /// <param name="userService">Служба пользователей.</param>
    /// <param name="logger">Служба логирования.</param>
    public class IdentityService(IUserService userService, ILogger<IdentityService> logger) : IIdentityService
    {
        private User _user;

        /// <summary>
        /// Получает имя пользователя учетной записи.
        /// </summary>
        /// <returns>Имя пользователя учетной записи.</returns>
        private static string GetAccountUserName()
        {
            return Environment.UserName;
        }

        /// <summary>
        /// Проверяет существование пользователя асинхронно.
        /// </summary>
        /// <returns>True, если пользователь существует, в противном случае - false.</returns>
        private async Task<bool> IsUserExistsAsync()
        {
            _user = await userService.GetCurrentUser(GetAccountUserName());

            return _user is not null;
        }

        /// <inheritdoc />
        public async Task<LoginResultType> LoginAsync()
        {
            try
            {
                if (!await IsUserExistsAsync())
                {
                    return LoginResultType.Unauthorized;
                }

                LoggedIn?.Invoke(this, _user);

                return LoginResultType.Success;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return LoginResultType.UnknownError;
            }
        }

        /// <inheritdoc />
        public void Logout()
        {
            LoggedOut?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />      
        public event EventHandler<User> LoggedIn;

        /// <inheritdoc />
        public event EventHandler LoggedOut;
    }
}
