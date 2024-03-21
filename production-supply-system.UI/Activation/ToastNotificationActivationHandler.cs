using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;

using Microsoft.Extensions.Configuration;

using UI_Interface.Contracts.Views;
using NavigationManager.Frame.Extension.WPF;
using UI_Interface.Contracts.Activation;

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
