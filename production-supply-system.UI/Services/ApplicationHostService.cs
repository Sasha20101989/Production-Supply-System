using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Cache.Manager.WPF;
using DAL.Models;
using Microsoft.Extensions.Hosting;
using NavigationManager.Frame.Extension.WPF;
using Theme.Manager.MahApps.WPF;
using UI_Interface.Contracts.Services;
using UI_Interface.Contracts.Views;
using UI_Interface.ViewModels.ViewModelsForPages;

namespace UI_Interface.Services
{
    /// <summary>
    /// Служба приложения, реализующая интерфейс IHostedService для управления жизненным циклом приложения.
    /// </summary>
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly INavigationManager _navigationManager;

        private readonly IСacheManager _cacheManager;

        private readonly IThemeManager _themeManager;

        private readonly IIdentityService _identityService;

        private readonly IUserDataService _userDataService;

        private IShellWindow _shellWindow;

        private ILogInWindow _logInWindow;

        private bool _isInitialized;

        /// <summary>
        /// Инициализирует новый экземпляр службы ApplicationHostService.
        /// </summary>
        /// <param name="serviceProvider">Поставщик служб для внедрения зависимостей.</param>
        /// <param name="navigationManager">Менеджер навигации.</param>
        /// <param name="themeManager">Менеджер тем оформления.</param>
        /// <param name="cacheManager">Менеджер кэша данных.</param>
        /// <param name="identityService">Служба аутентификации.</param>
        /// <param name="userDataService">Служба данных пользователя.</param>
        public ApplicationHostService(IServiceProvider serviceProvider,
            INavigationManager navigationManager,
            IThemeManager themeManager,
            IСacheManager cacheManager,
            IIdentityService identityService,
            IUserDataService userDataService)
        {
            _serviceProvider = serviceProvider;
            _navigationManager = navigationManager;
            _themeManager = themeManager;
            _cacheManager = cacheManager;
            _identityService = identityService;
            _userDataService = userDataService;
        }

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
                _logInWindow = _serviceProvider.GetService(typeof(ILogInWindow)) as ILogInWindow;
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
            _cacheManager.PersistData();
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
                _cacheManager.RestoreData();
                _themeManager.InitializeTheme();
                _themeManager.InitializeColor();
                _userDataService.Initialize();
                _identityService.LoggedIn += OnLoggedIn;
                _identityService.LoggedOut += OnLoggedOut;
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
                _shellWindow = _serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;
                _navigationManager.Initialize(_shellWindow.GetNavigationFrame());
                _shellWindow.ShowWindow();
                _ = _navigationManager.NavigateTo(typeof(MainViewModel).FullName);
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
            _logInWindow = _serviceProvider.GetService(typeof(ILogInWindow)) as ILogInWindow;
            _logInWindow.ShowWindow();

            _shellWindow.CloseWindow();
            _navigationManager.UnsubscribeNavigation();
        }
    }
}
