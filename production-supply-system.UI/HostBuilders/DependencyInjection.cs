using BLL.Contracts;
using BLL.Services;

using Cache.Manager.WPF;

using File.Manager;

using MahApps.Metro.Controls.Dialogs;

using Microsoft.Extensions.DependencyInjection;

using NavigationManager.Frame.Extension.WPF;

using PageManager.WPF;

using Theme.Manager.MahApps.WPF;

using UI_Interface.Contracts;
using UI_Interface.Contracts.Services;
using UI_Interface.Contracts.Views;
using UI_Interface.Multilang;
using UI_Interface.Services;
using UI_Interface.ViewModels.ViewModelsForPages;
using UI_Interface.Views;

namespace UI_Interface.HostBuilders
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            _ = services.AddSingleton<IUserService, UserService>();

            _ = services.AddSingleton<IDocumentService, DocumentService>();

            _ = services.AddSingleton<IProcessService, ProcessService>();

            _ = services.AddSingleton<IExcelService, ExcelService>();

            _ = services.AddSingleton<IStaticDataService, StaticDataService>();

            _ = services.AddSingleton<IDeliveryService, DeliveryService>();

            _ = services.AddSingleton<IBOMService, BOMService>();

            _ = services.AddSingleton<IExportProceduresService, ExportProceduresService>();

            return services;
        }

        public static IServiceCollection AddViewsAndViewModels(this IServiceCollection services)
        {
            _ = services.AddTransient<LogInViewModel>();
            _ = services.AddTransient<ILogInWindow, LogInWindow>();

            _ = services.AddTransient<ShellViewModel>();
            _ = services.AddTransient<IShellWindow, ShellWindow>();

            _ = services.AddSingleton<MainViewModel>();
            _ = services.AddSingleton<MainPage>();

            _ = services.AddTransient<SettingsViewModel>();
            _ = services.AddTransient<SettingsPage>();

            _ = services.AddTransient<DocumentMapperViewModel>();
            _ = services.AddTransient<DocumentMapperPage>();

            _ = services.AddTransient<DocumentMapperDetailViewModel>();
            _ = services.AddTransient<DocumentMapperDetailPage>();

            _ = services.AddSingleton<DeliveryViewModel>();
            _ = services.AddSingleton<DeliveryPage>();

            _ = services.AddTransient<EditDeliveryViewModel>();
            _ = services.AddTransient<EditDeliveryPage>();

            return services;
        }

        public static IServiceCollection AddUiServices(this IServiceCollection services)
        {
            _ = services.AddSingleton<IIdentityService, IdentityService>();
            _ = services.AddSingleton<IUserDataService, UserDataService>();

            _ = services.AddSingleton<IMultilangManager, MultilangManager>();
            _ = services.AddSingleton<IThemeManager, ThemeManager>();
            _ = services.AddSingleton<IСacheManager, СacheManager>();
            _ = services.AddSingleton<IFileManager, FileManager>();
            _ = services.AddSingleton<IPageManager, PageManager.WPF.PageManager>();
            _ = services.AddSingleton<INavigationManager, NavigationManager.Frame.Extension.WPF.NavigationManager>();
            _ = services.AddSingleton<IDialogCoordinator, DialogCoordinator>();
            _ = services.AddSingleton<IToastNotificationsService, ToastNotificationsService>();
            _ = services.AddSingleton<ISystemService, SystemService>();

            return services;
        }
    }
}
