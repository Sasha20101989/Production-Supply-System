using System.Windows.Controls;
using UI_Interface.ViewModels;

namespace UI_Interface.Views
{
    /// <summary>
    /// Логика взаимодействия для ValidationPage.xaml
    /// </summary>
    public partial class FileValidationPage : Page
    {
        public FileValidationPage(FileValidationViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
