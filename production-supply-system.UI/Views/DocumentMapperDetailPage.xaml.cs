using System.Windows.Controls;
using UI_Interface.ViewModels.ViewModelsForPages;

namespace UI_Interface.Views
{
    /// <summary>
    /// Interaction logic for DocumentMapperContentPage.xaml
    /// </summary>
    public partial class DocumentMapperDetailPage : Page
    {
        public DocumentMapperDetailPage(DocumentMapperDetailViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
