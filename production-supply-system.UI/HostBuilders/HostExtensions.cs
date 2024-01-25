using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PageManager.WPF;
using UI_Interface.ViewModels.ViewModelsForPages;
using UI_Interface.ViewModels;
using UI_Interface.Views;
using System;

namespace UI_Interface.HostBuilders
{
    public static class HostExtensions
    {
        public static IHost ConfigurePages(this IHost host)
        {
            ConfigurePages(host.Services);

            return host;
        }

        private static void ConfigurePages(IServiceProvider serviceProvider)
        {
            IPageManager pageService = serviceProvider.GetRequiredService<IPageManager>();

            pageService.Configure<MainViewModel, MainPage>();

            pageService.Configure<SettingsViewModel, SettingsPage>();

            pageService.Configure<DocumentMapperViewModel, DocumentMapperPage>();

            pageService.Configure<DocumentMapperDetailViewModel, DocumentMapperDetailPage>();

            pageService.Configure<DeliveryPageViewModel, DeliveryPage>();

            pageService.Configure<MasterViewModel, MasterPage>();

            pageService.Configure<FileValidationViewModel, FileValidationPage>();
        }
    }
}
