using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Extensions.Configuration;

using UI_Interface.Contracts.Activation;
using UI_Interface.Contracts.Views;

namespace UI_Interface.Activation
{
    public class ToastNotificationActivationHandler(IConfiguration config) : IActivationHandler
    {
        public const string ActivationArguments = "ToastNotificationActivationArguments";

        public bool CanHandle()
        {
            return !string.IsNullOrEmpty(config[ActivationArguments]);
        }

        public async Task HandleAsync()
        {
            if (!Application.Current.Windows.OfType<IShellWindow>().Any())
            {
                // Here you can get an instance of the ShellWindow and choose navigate
                // to a specific page depending on the toast notification arguments
            }
            else
            {
                _ = App.Current.MainWindow.Activate();

                if (App.Current.MainWindow.WindowState == WindowState.Minimized)
                {
                    App.Current.MainWindow.WindowState = WindowState.Normal;
                }
            }

            await Task.CompletedTask;
        }
    }
}
