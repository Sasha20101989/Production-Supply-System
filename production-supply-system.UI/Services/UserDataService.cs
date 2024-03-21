using System;
using System.IO;
using Cache.Manager.WPF;
using DAL.Models;
using File.Manager;
using Microsoft.Extensions.Options;
using UI_Interface.Contracts.Services;
using UI_Interface.ViewModels;

namespace UI_Interface.Services
{
    /// <summary>
    /// Сервис управления данными пользователя.
    /// </summary>
    /// <remarks>
    /// Инициализирует новый экземпляр службы управления данными пользователя.
    /// </remarks>
    /// <param name="fileManager">Менеджер файлов.</param>
    /// <param name="identityService">Служба аутентификации.</param>
    /// <param name="appConfig">Конфигурации приложения.</param>
    public class UserDataService(IFileManager fileManager, IIdentityService identityService, IOptions<AppConfig> appConfig) : IUserDataService
    {
        private readonly AppConfig _appConfig = appConfig.Value;

        private readonly string _localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private UserViewModel _userViewModel;

        /// <summary>
        /// Получает имя пользователя учетной записи.
        /// </summary>
        /// <returns>Имя пользователя учетной записи.</returns>
        private static string GetAccountUserName()
        {
            return Environment.UserName;
        }

        /// <summary>
        /// Создает объект UserViewModel на основе данных пользователя.
        /// </summary>
        /// <param name="userData">Данные пользователя.</param>
        /// <returns>Объект UserViewModel.</returns>
        private static UserViewModel GetUserViewModelFromData(User userData)
        {
            return userData is null
                ? null
                : new UserViewModel()
            {
                Name = userData.Name,
                Patronymic = userData.Patronymic,
                Account = userData.Account
            };
        }

        /// <summary>
        /// Получает объект UserViewModel с данными по умолчанию.
        /// </summary>
        /// <returns>Объект UserViewModel с данными по умолчанию.</returns>
        private static UserViewModel GetDefaultUserData()
        {
            return new UserViewModel()
            {
                Name = GetAccountUserName()
            };
        }

        /// <summary>
        /// Обработчик события входа пользователя.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="user">Данные пользователя.</param>
        private void OnLoggedIn(object sender, User user)
        {
            if (user is not null)
            {
                SaveUserToCache(user);
                _userViewModel = GetUserViewModelFromData(user);

                UserDataUpdated?.Invoke(this, _userViewModel);
            }
        }

        /// <summary>
        /// Обработчик события выхода пользователя.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void OnLoggedOut(object sender, EventArgs e)
        {
            _userViewModel = null;

            string folderPath = Path.Combine(_localAppData, _appConfig.ConfigurationsFolder);

            fileManager.Save<User>(folderPath, _appConfig.UserFileName, null);
        }

        /// <summary>
        /// Сохраняет данные пользователя в кэш.
        /// </summary>
        /// <param name="userData">Данные пользователя.</param>
        private void SaveUserToCache(User userData)
        {
            string folderPath = Path.Combine(_localAppData, _appConfig.ConfigurationsFolder);

            fileManager.Save(folderPath, _appConfig.UserFileName, userData);
        }

        /// <summary>
        /// Получает объект UserViewModel из кэша.
        /// </summary>
        /// <returns>Объект UserViewModel из кэша.</returns>
        private UserViewModel GetUserViewModelFromCache()
        {
            string folderPath = Path.Combine(_localAppData, _appConfig.ConfigurationsFolder);

            User cacheData = fileManager.Read<User>(folderPath, _appConfig.UserFileName);

            return GetUserViewModelFromData(cacheData);
        }

        /// <inheritdoc />
        public void Initialize()
        {
            identityService.LoggedIn += OnLoggedIn;
            identityService.LoggedOut += OnLoggedOut;
        }

        /// <inheritdoc />
        public UserViewModel GetUser()
        {
            if (_userViewModel is null)
            {
                _userViewModel = GetUserViewModelFromCache();

                _userViewModel ??= GetDefaultUserData();
            }

            return _userViewModel;
        }

        /// <inheritdoc />
        public event EventHandler<UserViewModel> UserDataUpdated;
    }
}
