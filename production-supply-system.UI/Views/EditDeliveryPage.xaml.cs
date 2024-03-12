using System.Windows.Controls;

using UI_Interface.ViewModels.ViewModelsForPages;

namespace UI_Interface.Views
{
    /// <summary>
    /// Логика взаимодействия для EditDeliveryPage.xaml
    /// </summary>
    public partial class EditDeliveryPage : Page
    {
        public EditDeliveryPage(EditDeliveryViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
