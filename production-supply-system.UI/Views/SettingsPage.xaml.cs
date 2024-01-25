using System.Windows.Controls;

using UI_Interface.ViewModels.ViewModelsForPages;

namespace UI_Interface.Views
{
    public partial class SettingsPage : Page
    {
        public SettingsPage(SettingsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
