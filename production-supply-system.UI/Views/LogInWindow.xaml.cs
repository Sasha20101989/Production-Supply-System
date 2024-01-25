using MahApps.Metro.Controls;

using UI_Interface.Contracts.Views;
using UI_Interface.ViewModels.ViewModelsForPages;

namespace UI_Interface.Views
{
    public partial class LogInWindow : MetroWindow, ILogInWindow
    {
        public LogInWindow(LogInViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
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
