using System.Windows.Controls;
using UI_Interface.ViewModels.ViewModelsForPages;

namespace UI_Interface.Views
{
    /// <summary>
    /// Логика взаимодействия для MasterPage.xaml
    /// </summary>
    public partial class MasterPage : Page
    {
        public MasterPage(MasterViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
