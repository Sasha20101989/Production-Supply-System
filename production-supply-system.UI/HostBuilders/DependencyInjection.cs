using DAL.Data.Repositories.Contracts;
using DAL.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
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
using DAL.Models;
using DAL.Models.Master;
using DAL.Models.Document;
using DAL.Models.Planning;
using DAL.Models.BOM;
using DAL.Models.Inbound;
using DAL.Models.Partscontrol;
using UI_Interface.Multilang;
using UI_Interface.Contracts;

namespace UI_Interface.HostBuilders
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSqlServerData(this IServiceCollection services)
        {
            _ = services.AddSingleton<UserSqlServerData>();
            _ = services.AddSingleton<DocumentSqlServerData>();
            _ = services.AddSingleton<DocumentColumnSqlServerData>();
            _ = services.AddSingleton<DocumentContentSqlServerData>();
            _ = services.AddSingleton<SectionSqlServerData>();
            _ = services.AddSingleton<ProcessSqlServerData>();
            _ = services.AddSingleton<ProcessStepSqlServerData>();
            _ = services.AddSingleton<ShipperSqlServerData>();
            _ = services.AddSingleton<CarrierSqlServerData>();
            _ = services.AddSingleton<TermsOfDeliverySqlServerData>();
            _ = services.AddSingleton<TransportTypeSqlServerData>();
            _ = services.AddSingleton<LocationSqlServerData>();
            _ = services.AddSingleton<LocationTypeSqlServerData>();
            _ = services.AddSingleton<PurchaseOrderSqlServerData>();
            _ = services.AddSingleton<PurchaseOrderTypeSqlServerData>();
            _ = services.AddSingleton<TransportSqlServerData>();
            _ = services.AddSingleton<InvoiceSqlServerData>();
            _ = services.AddSingleton<LotSqlServerData>();
            _ = services.AddSingleton<ContainerTypeSqlServerData>();
            _ = services.AddSingleton<ContainerSqlServerData>();
            _ = services.AddSingleton<PartInContainerSqlServerData>();
            _ = services.AddSingleton<PartInInvoiceSqlServerData>();
            _ = services.AddSingleton<CustomsPartsSqlServerData>();
            _ = services.AddSingleton<CaseSqlServerData>();
            _ = services.AddSingleton<PackingTypeSqlServerData>();
            _ = services.AddSingleton<BodyModelVariantSqlServerData>();
            _ = services.AddSingleton<PartTypeSqlServerData>();
            _ = services.AddSingleton<VinContainerSqlServerData>();
            _ = services.AddSingleton<CustomsClearanceSqlServerData>();
            _ = services.AddSingleton<BomPartSqlServerData>();
            _ = services.AddSingleton<TracingSqlServerData>();

            return services;
        }

        public static IServiceCollection ConfigureSqlServerData(this IServiceCollection services)
        {
            _ = services.AddSingleton<ISqlDataAccess, SqlDataAccess>();

            _ = services.AddSingleton<ISqlMapper, DapperSqlMapper>();

            _ = services.AddSingleton<IConfigurationWrapper, ConfigurationWrapper>();

            _ = services.AddSingleton<IRepository<User>>(provider =>
            {
                UserSqlServerData sqlServerData = provider.GetRequiredService<UserSqlServerData>();

                return new Repository<User>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<Docmapper>>(provider =>
            {
                DocumentSqlServerData sqlServerData = provider.GetRequiredService<DocumentSqlServerData>();

                return new Repository<Docmapper>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<DocmapperColumn>>(provider =>
            {
                DocumentColumnSqlServerData sqlServerData = provider.GetRequiredService<DocumentColumnSqlServerData>();

                return new Repository<DocmapperColumn>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<DocmapperContent>>(provider =>
            {
                DocumentContentSqlServerData sqlServerData = provider.GetRequiredService<DocumentContentSqlServerData>();

                return new Repository<DocmapperContent>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<Section>>(provider =>
            {
                SectionSqlServerData sqlServerData = provider.GetRequiredService<SectionSqlServerData>();

                return new Repository<Section>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<Process>>(provider =>
            {
                ProcessSqlServerData sqlServerData = provider.GetRequiredService<ProcessSqlServerData>();

                return new Repository<Process>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<ProcessStep>>(provider =>
            {
                ProcessStepSqlServerData sqlServerData = provider.GetRequiredService<ProcessStepSqlServerData>();

                return new Repository<ProcessStep>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<Shipper>>(provider =>
            {
                ShipperSqlServerData sqlServerData = provider.GetRequiredService<ShipperSqlServerData>();

                return new Repository<Shipper>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<Carrier>>(provider =>
            {
                CarrierSqlServerData sqlServerData = provider.GetRequiredService<CarrierSqlServerData>();

                return new Repository<Carrier>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<TermsOfDelivery>>(provider =>
            {
                TermsOfDeliverySqlServerData sqlServerData = provider.GetRequiredService<TermsOfDeliverySqlServerData>();

                return new Repository<TermsOfDelivery>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<TypesOfTransport>>(provider =>
            {
                TransportTypeSqlServerData sqlServerData = provider.GetRequiredService<TransportTypeSqlServerData>();

                return new Repository<TypesOfTransport>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<Location>>(provider =>
            {
                LocationSqlServerData sqlServerData = provider.GetRequiredService<LocationSqlServerData>();

                return new Repository<Location>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<TypesOfLocation>>(provider =>
            {
                LocationTypeSqlServerData sqlServerData = provider.GetRequiredService<LocationTypeSqlServerData>();

                return new Repository<TypesOfLocation>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<PurchaseOrder>>(provider =>
            {
                PurchaseOrderSqlServerData sqlServerData = provider.GetRequiredService<PurchaseOrderSqlServerData>();

                return new Repository<PurchaseOrder>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<Transport>>(provider =>
            {
                TransportSqlServerData sqlServerData = provider.GetRequiredService<TransportSqlServerData>();

                return new Repository<Transport>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<Invoice>>(provider =>
            {
                InvoiceSqlServerData sqlServerData = provider.GetRequiredService<InvoiceSqlServerData>();

                return new Repository<Invoice>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<Lot>>(provider =>
            {
                LotSqlServerData sqlServerData = provider.GetRequiredService<LotSqlServerData>();

                return new Repository<Lot>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<TypesOfContainer>>(provider =>
            {
                ContainerTypeSqlServerData sqlServerData = provider.GetRequiredService<ContainerTypeSqlServerData>();

                return new Repository<TypesOfContainer>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<ContainersInLot>>(provider =>
            {
                ContainerSqlServerData sqlServerData = provider.GetRequiredService<ContainerSqlServerData>();

                return new Repository<ContainersInLot>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<PartsInContainer>>(provider =>
            {
                PartInContainerSqlServerData sqlServerData = provider.GetRequiredService<PartInContainerSqlServerData>();

                return new Repository<PartsInContainer>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<CustomsPart>>(provider =>
            {
                CustomsPartsSqlServerData sqlServerData = provider.GetRequiredService<CustomsPartsSqlServerData>();

                return new Repository<CustomsPart>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<Case>>(provider =>
            {
                CaseSqlServerData sqlServerData = provider.GetRequiredService<CaseSqlServerData>();

                return new Repository<Case>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<TypesOfPacking>>(provider =>
            {
                PackingTypeSqlServerData sqlServerData = provider.GetRequiredService<PackingTypeSqlServerData>();

                return new Repository<TypesOfPacking>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<BodyModelVariant>>(provider =>
            {
                BodyModelVariantSqlServerData sqlServerData = provider.GetRequiredService<BodyModelVariantSqlServerData>();

                return new Repository<BodyModelVariant>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<TypesOfPart>>(provider =>
            {
                PartTypeSqlServerData sqlServerData = provider.GetRequiredService<PartTypeSqlServerData>();

                return new Repository<TypesOfPart>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<VinsInContainer>>(provider =>
            {
                VinContainerSqlServerData sqlServerData = provider.GetRequiredService<VinContainerSqlServerData>();

                return new Repository<VinsInContainer>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<CustomsClearance>>(provider =>
            {
                CustomsClearanceSqlServerData sqlServerData = provider.GetRequiredService<CustomsClearanceSqlServerData>();

                return new Repository<CustomsClearance>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<PartsInInvoice>>(provider =>
            {
                PartInInvoiceSqlServerData sqlServerData = provider.GetRequiredService<PartInInvoiceSqlServerData>();

                return new Repository<PartsInInvoice>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<BomPart>>(provider =>
            {
                BomPartSqlServerData sqlServerData = provider.GetRequiredService<BomPartSqlServerData>();

                return new Repository<BomPart>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<Tracing>>(provider =>
            {
                TracingSqlServerData sqlServerData = provider.GetRequiredService<TracingSqlServerData>();

                return new Repository<Tracing>(sqlServerData);
            });

            _ = services.AddSingleton<IRepository<TypesOfOrder>>(provider =>
            {
                PurchaseOrderTypeSqlServerData sqlServerData = provider.GetRequiredService<PurchaseOrderTypeSqlServerData>();

                return new Repository<TypesOfOrder>(sqlServerData);
            });

            return services;
        }

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
