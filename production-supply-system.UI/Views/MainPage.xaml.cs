using System.Windows.Controls;

using UI_Interface.ViewModels.ViewModelsForPages;

namespace UI_Interface.Views
{
    public partial class MainPage : Page
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
