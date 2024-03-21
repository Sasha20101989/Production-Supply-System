using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using NavigationManager.Frame.Extension.WPF;

using System;
using System.Collections.Generic;
using System.Linq;

using Theme.Manager.MahApps.WPF;

using UI_Interface.Contracts;
using UI_Interface.Contracts.Services;
using UI_Interface.Multilang;
using UI_Interface.Properties;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    public partial class SettingsViewModel(
        IThemeManager themeManager, 
        IUserDataService userDataService, 
        IIdentityService identityService, 
        IMultilangManager multilangManager, 
        IToastNotificationsService toastNotificationsService) : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private AppTheme _theme;

        [ObservableProperty]
        private Languages _language;

        [ObservableProperty]
        private string _color;

        [ObservableProperty]
        private UserViewModel _user;

        [ObservableProperty]
        private IEnumerable<AccentColorMenuData> _accentColors = ControlzEx.Theming.ThemeManager.Current.Themes
                                            .GroupBy(x => x.ColorScheme)
                                            .OrderBy(a => a.Key)
                                            .Select(a => new AccentColorMenuData { Name = a.Key, ColorBrush = a.FirstOrDefault().ShowcaseBrush })
                                            .ToList();

        public void OnNavigatedTo(object parameter)
        {
            Theme = themeManager.GetCurrentTheme();
            Color = themeManager.GetCurrentColor();
            Language = multilangManager.GetCurrentLanguage();
            AccentColors = themeManager.GetColors();
            identityService.LoggedOut += OnLoggedOut;
            userDataService.UserDataUpdated += OnUserDataUpdated;
            User = userDataService.GetUser();
        }

        public void OnNavigatedFrom()
        {
            UnregisterEvents();
        }

        [RelayCommand]
        private void SetTheme(string themeName)
        {
            AppTheme theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
            themeManager.SetTheme(theme);
        }

        [RelayCommand]
        private void SetLanguage(string newLanguage)
        {
            Languages language = (Languages)Enum.Parse(typeof(Languages), newLanguage);

            toastNotificationsService.ShowToastNotificationMessage(Resources.ShellChangeLanguage, Resources.ShellMessageChangeLanguage);

            multilangManager.SetLanguage(language);
        }

        [RelayCommand]
        private void LogOut()
        {
            identityService.Logout();
        }

        private void UnregisterEvents()
        {
            identityService.LoggedOut -= OnLoggedOut;
            userDataService.UserDataUpdated -= OnUserDataUpdated;
        }

        private void OnUserDataUpdated(object sender, UserViewModel userData)
        {
            User = userData;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            UnregisterEvents();
        }
    }
}
