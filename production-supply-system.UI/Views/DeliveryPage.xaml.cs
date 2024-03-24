using System.Windows.Controls;
using System.Windows.Input;

using UI_Interface.ViewModels;
using UI_Interface.ViewModels.ViewModelsForPages;

namespace UI_Interface.Views
{
    /// <summary>
    /// Логика взаимодействия для DeliveryPage.xaml
    /// </summary>
    public partial class DeliveryPage : Page
    {
        private readonly DeliveryViewModel _viewModel;

        public DeliveryPage(DeliveryViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            DataContext = viewModel;
        }

        private void ListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is LotViewModel lotViewModel)
            {
                _viewModel.NavigateToDetailsCommand.Execute(lotViewModel.Lot);
            }
        }
    }
}
