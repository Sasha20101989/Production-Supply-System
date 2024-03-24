using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Cache.Manager.WPF;

using Microsoft.Extensions.Hosting;

using NavigationManager.Frame.Extension.WPF;

using production_supply_system.EntityFramework.DAL.Models.UsersSchema;

using Theme.Manager.MahApps.WPF;

using UI_Interface.Contracts.Services;
using UI_Interface.Contracts.Views;
using UI_Interface.Multilang;
using UI_Interface.ViewModels.ViewModelsForPages;

namespace UI_Interface.Services
{
    /// <summary>
    /// Служба приложения, реализующая интерфейс IHostedService для управления жизненным циклом приложения.
    /// </summary>
    public class ApplicationHostService(IServiceProvider serviceProvider,
            INavigationManager navigationManager,
            IThemeManager themeManager,
            IСacheManager cacheManager,
            IIdentityService identityService,
            IMultilangManager multilangManager,
            IUserDataService userDataService) : IHostedService
    {
        private IShellWindow _shellWindow;

        private ILogInWindow _logInWindow;

        private bool _isInitialized;

        /// <summary>
        /// Запускает службу инициализации приложения.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены задачи.</param>
        /// <returns>Задача, представляющая асинхронный запуск службы.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitializeAsync();

            if (!_isInitialized)
            {
                _logInWindow = serviceProvider.GetService(typeof(ILogInWindow)) as ILogInWindow;
                _logInWindow.ShowWindow();
                await StartupAsync();
                _isInitialized = true;
            }

            return;
        }

        /// <summary>
        /// Останавливает службу и сохраняет данные кэша.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены задачи.</param>
        /// <returns>Задача, представляющая асинхронную остановку службы.</returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            cacheManager.PersistData();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Инициализирует службу, восстанавливает данные из кэша, устанавливает тему оформления
        /// и инициализирует службы пользователя и аутентификации.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную инициализацию службы.</returns>
        public async Task InitializeAsync()
        {
            if (!_isInitialized)
            {
                cacheManager.RestoreData();
                multilangManager.InitializeLanguage();
                themeManager.InitializeTheme();
                themeManager.InitializeColor();
                userDataService.Initialize();
                identityService.LoggedIn += OnLoggedIn;
                identityService.LoggedOut += OnLoggedOut;
                await Task.CompletedTask;
            }
        }

        /// <summary>
        /// Обрабатывает активацию приложения, инициализируя главное окно и осуществляя навигацию к основной модели представления.
        /// </summary>
        private void HandleActivation()
        {

            if (!Application.Current.Windows.OfType<IShellWindow>().Any())
            {
                _shellWindow = serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;
                navigationManager.Initialize(_shellWindow.GetNavigationFrame());
                _shellWindow.ShowWindow();
                _ = navigationManager.NavigateTo(typeof(MainViewModel).FullName);
            }
        }

        /// <summary>
        /// Обработчик события входа пользователя.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные о пользователе.</param>
        private void OnLoggedIn(object sender, User e)
        {
            HandleActivation();
            _logInWindow.CloseWindow();
        }

        /// <summary>
        /// Асинхронный метод для дополнительной инициализации при старте приложения.
        /// </summary>
        /// <returns>Задача, представляющая асинхронный старт приложения.</returns>
        private async Task StartupAsync()
        {
            if (!_isInitialized)
            {
                await Task.CompletedTask;
            }
        }

        /// <summary>
        /// Обработчик события выхода пользователя.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void OnLoggedOut(object sender, EventArgs e)
        {
            _logInWindow = serviceProvider.GetService(typeof(ILogInWindow)) as ILogInWindow;
            _logInWindow.ShowWindow();

            _shellWindow.CloseWindow();
            navigationManager.UnsubscribeNavigation();
        }
    }
}
