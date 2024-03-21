using CommunityToolkit.Mvvm.ComponentModel;

using NavigationManager.Frame.Extension.WPF;

using UI_Interface.Multilang;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    public class MainViewModel(IMultilangManager multilangManager) : ObservableObject, INavigationAware
    {
        public void OnNavigatedTo(object parameter)
        {
            multilangManager.InitializeLanguage();
        }

        public void OnNavigatedFrom()
        {

        }
    }
}
