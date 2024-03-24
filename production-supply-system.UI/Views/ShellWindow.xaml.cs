using System.Windows.Controls;

using MahApps.Metro.Controls;

using UI_Interface.Contracts.Views;
using UI_Interface.ViewModels.ViewModelsForPages;

namespace UI_Interface.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow
    {
        public ShellWindow(ShellViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public Frame GetNavigationFrame()
        {
            return shellFrame;
        }

        public void ShowWindow()
        {
            Show();
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
