using System.Windows.Controls;

using UI_Interface.ViewModels.ViewModelsForPages;

namespace UI_Interface.Views
{
    /// <summary>
    /// Interaction logic for DocumentMapperPage.xaml
    /// </summary>
    public partial class DocumentMapperPage : Page
    {
        public DocumentMapperPage(DocumentMapperViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
