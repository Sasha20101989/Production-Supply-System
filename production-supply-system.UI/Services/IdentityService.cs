using System;
using System.Threading.Tasks;
using System.Windows;

using BLL.Contracts;
using DAL.Models;

using Microsoft.Extensions.Logging;

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
        private readonly ILogger<IdentityService> _logger;
        private User _user;

        /// <summary>
        /// Инициализирует новый экземпляр службы аутентификации.
        /// </summary>
        /// <param name="userService">Служба пользователей.</param>
        /// <param name="logger">Служба логирования.</param>
        public IdentityService(IUserService userService, ILogger<IdentityService> logger)
        {
            _userService = userService;
            _logger = logger;
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
                bool isAccessAllowed = await _userService.IsAccessAllowedAsync();

                if (!isAccessAllowed)
                {
                    return LoginResultType.NotConnectionToDb;
                }

                if (!await IsUserExistsAsync())
                {
                    return LoginResultType.Unauthorized;
                }

                LoggedIn?.Invoke(this, _user);

                return LoginResultType.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

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
