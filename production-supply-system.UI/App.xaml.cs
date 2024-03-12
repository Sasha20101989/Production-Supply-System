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
            string appLocation = Path.GetDirectoryName(System.AppContext.BaseDirectory);

            _host = Host.CreateDefaultBuilder(e.Args)
                    .ConfigureAppConfiguration(c =>
                    {
                        _ = c.SetBasePath(appLocation);
                    })
                    .ConfigureServices(ConfigureServices)
                    .Build();

            _ = _host.ConfigurePages();

            DispatcherUnhandledException += OnDispatcherUnhandledException;

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
            e.Handled = true;
        }
    }
}
