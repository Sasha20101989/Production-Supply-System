using System;
using System.Threading.Tasks;

using BLL.Contracts;

using DAL.Models;

using UI_Interface.Contracts.Services;
using UI_Interface.Helpers;

namespace UI_Interface.Services
{
    /// <summary>
    /// Сервис аутентификации пользователя.
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly IUserService _userService;
        private User _user;

        /// <summary>
        /// Инициализирует новый экземпляр службы аутентификации.
        /// </summary>
        /// <param name="userService">Служба пользователей.</param>
        public IdentityService(IUserService userService)
        {
            _userService = userService;
        }

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
            _user = await _userService.GetUserInfoAsync(GetAccountUserName());

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
            catch (Exception)
            {
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
