using System.Windows.Controls;

using UI_Interface.ViewModels.ViewModelsForPages;

namespace UI_Interface.Views
{
    /// <summary>
    /// Логика взаимодействия для DeliveryPage.xaml
    /// </summary>
    public partial class DeliveryPage : Page
    {
        public DeliveryPage(DeliveryPageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
