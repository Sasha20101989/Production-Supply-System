using DAL.Data.Repositories.Contracts;
using DAL.Data.Repositories;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using DAL.Data.Contracts;
using DAL.Data.SqlServer;
using DAL.DataAccess.Contracts;
using DAL.DataAccess;
using DAL.DbAccess.Contracts;
using DAL.DbAccess;
using DAL.Helpers.Contracts;
using DAL.Helpers;
using BLL.Contracts;
using BLL.Services;
using UI_Interface.Contracts.Views;
using UI_Interface.ViewModels.ViewModelsForPages;
using UI_Interface.ViewModels;
using UI_Interface.Views;
using Cache.Manager.WPF;
using File.Manager;
using MahApps.Metro.Controls.Dialogs;
using NavigationManager.Frame.Extension.WPF;
using PageManager.WPF;
using Theme.Manager.MahApps.WPF;
using UI_Interface.Contracts.Services;
using UI_Interface.Services;

namespace UI_Interface.HostBuilders
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            _ = services.AddSingleton<IUserRepository, UserRepository>();

            _ = services.AddSingleton<IDocumentMapperRepository, DocumentMapperRepository>();

            _ = services.AddSingleton<ISectionRepository, SectionRepository>();

            _ = services.AddSingleton<IProcessStepsRepository, ProcessStepsRepository>();

            return services;
        }

        public static IServiceCollection AddSqlServerData(this IServiceCollection services)
        {
            _ = services.AddSingleton<ISqlDataAccess, SqlDataAccess>();

            _ = services.AddSingleton<ISqlMapper, DapperSqlMapper>();

            _ = services.AddSingleton<IConfigurationWrapper, ConfigurationWrapper>();

            _ = services.AddSingleton<IUserData, UserSqlServerData>();

            _ = services.AddSingleton<IDocumentData, DocumentSqlServerData>();

            _ = services.AddSingleton<IDocumentColumnData, DocumentColumnSqlServerData>();

            _ = services.AddSingleton<IDocumentContentData, DocumentContentSqlServerData>();

            _ = services.AddSingleton<ISectionData, SectionSqlServerData>();

            _ = services.AddSingleton<IProcessStepData, ProcessStepSqlServerData>();

            return services;
        }

        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            _ = services.AddSingleton<IUserService, UserService>();

            _ = services.AddSingleton<IDocumentService, DocumentService>();

            _ = services.AddSingleton<IProcessService, ProcessService>();

            _ = services.AddSingleton<IExcelService, ExcelService>();

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

            _ = services.AddSingleton<DeliveryPageViewModel>();
            _ = services.AddSingleton<DeliveryPage>();

            _ = services.AddTransient<MasterViewModel>();
            _ = services.AddTransient<MasterPage>();

            _ = services.AddSingleton<FileValidationViewModel>();
            _ = services.AddSingleton<FileValidationPage>();

            return services;
        }

        public static IServiceCollection AddUiServices(this IServiceCollection services)
        {
            _ = services.AddSingleton<IIdentityService, IdentityService>();
            _ = services.AddSingleton<IUserDataService, UserDataService>();

            _ = services.AddSingleton<IThemeManager, ThemeManager>();
            _ = services.AddSingleton<IСacheManager, СacheManager>();
            _ = services.AddSingleton<IFileManager, FileManager>();
            _ = services.AddSingleton<IPageManager, PageManager.WPF.PageManager>();
            _ = services.AddSingleton<INavigationManager, NavigationManager.Frame.Extension.WPF.NavigationManager>();
            _ = services.AddSingleton<IDialogCoordinator, DialogCoordinator>();

            return services;
        }
    }
}
