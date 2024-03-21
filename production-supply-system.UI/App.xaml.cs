using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows.Threading;
using NLog.Extensions.Logging;
using System.IO;
using UI_Interface.Services;
using Cache.Manager.WPF;
using UI_Interface.HostBuilders;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;
using CommunityToolkit.WinUI.Notifications;
using UI_Interface.Activation;
using System.Collections.Generic;
using UI_Interface.Multilang;

namespace UI_Interface
{
    public partial class App : Application
    {
        private IHost _host;

        public App()
        {

        }

        public IConfiguration Configuration { get; }

        public T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            ToastNotificationManagerCompat.OnActivated += (toastArgs) => {
                _ = Current.Dispatcher.Invoke(async () =>
                {
                    IConfiguration config = GetService<IConfiguration>();
                    config[ToastNotificationActivationHandler.ActivationArguments] = toastArgs.Argument;
                    await _host.StartAsync();
                });
            };

            Dictionary<string, string> activationArgs = new()
            {
                { ToastNotificationActivationHandler.ActivationArguments, string.Empty }
            };

            string appLocation = Path.GetDirectoryName(System.AppContext.BaseDirectory);

            _host = Host.CreateDefaultBuilder(e.Args)
                    .ConfigureAppConfiguration(c =>
                    {
                        _ = c.SetBasePath(appLocation);
                        _ = c.AddInMemoryCollection(activationArgs);
                    })
                    .ConfigureServices(ConfigureServices)
                    .Build();

            _ = _host.ConfigurePages();

            DispatcherUnhandledException += OnDispatcherUnhandledException;

            if (ToastNotificationManagerCompat.WasCurrentProcessToastActivated())
            {
                // ToastNotificationActivator code will run after this completes and will show a window if necessary.
                return;
            }

            await _host.StartAsync();
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            _ = services.AddHostedService<ApplicationHostService>();

            _ = services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));

            _ = services.AddLogging(builder =>
            {
                _ = builder.ClearProviders();
                _ = builder.SetMinimumLevel(LogLevel.Trace);
                _ = builder.AddNLog();
            });

            _ = services.AddSqlServerData();

            _ = services.ConfigureSqlServerData();

            _ = services.AddBusinessLogic();

            _ = services.AddViewsAndViewModels();     
            
            _ = services.AddUiServices();
        }

        public async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            _host = null;
        }

        public void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ILogger<App> logger = _host.Services.GetRequiredService<ILogger<App>>();

            logger.LogError($"Unhandled exception occured: {e.Exception}");

            //TODO: Убрать в продакшене
            //e.Handled = true;
        }
    }
}
