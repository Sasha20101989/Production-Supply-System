using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using NavigationManager.Frame.Extension.WPF;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Theme.Manager.MahApps.WPF;

using UI_Interface.Contracts.Services;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    public class SettingsViewModel : ObservableObject, INavigationAware
    {
        private readonly IThemeManager _themeManager;

        private readonly IUserDataService _userDataService;

        private readonly IIdentityService _identityService;

        private AppTheme _theme;

        private string _color;

        private UserViewModel _user;

        public SettingsViewModel(IThemeManager themeManager, IUserDataService userDataService, IIdentityService identityService)
        {
            _themeManager = themeManager;

            _userDataService = userDataService;

            _identityService = identityService;

            SetThemeCommand = new RelayCommand<string>(OnSetTheme);

            LogOutCommand = new RelayCommand(OnLogOut);

            AccentColors = ControlzEx.Theming.ThemeManager.Current.Themes
                                            .GroupBy(x => x.ColorScheme)
                                            .OrderBy(a => a.Key)
                                            .Select(a => new AccentColorMenuData { Name = a.Key, ColorBrush = a.FirstOrDefault().ShowcaseBrush })
                                            .ToList();
        }

        public AppTheme Theme
        {
            get => _theme;
            set => _ = SetProperty(ref _theme, value);
        }

        public string Color
        {
            get => _color;
            set => _ = SetProperty(ref _color, value);
        }

        public UserViewModel User
        {
            get => _user;
            set => _ = SetProperty(ref _user, value);
        }

        public IEnumerable<AccentColorMenuData> AccentColors { get; set; }

        public ICommand SetThemeCommand { get; }

        public ICommand LogOutCommand { get; }

        public void OnNavigatedTo(object parameter)
        {
            Theme = _themeManager.GetCurrentTheme();
            Color = _themeManager.GetCurrentColor();
            AccentColors = _themeManager.GetColors();
            _identityService.LoggedOut += OnLoggedOut;
            _userDataService.UserDataUpdated += OnUserDataUpdated;
            User = _userDataService.GetUser();
        }

        public void OnNavigatedFrom()
        {
            UnregisterEvents();
        }

        private void UnregisterEvents()
        {
            _identityService.LoggedOut -= OnLoggedOut;
            _userDataService.UserDataUpdated -= OnUserDataUpdated;
        }

        private void OnSetTheme(string themeName)
        {
            AppTheme theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
            _themeManager.SetTheme(theme);
        }

        private void OnLogOut()
        {
            _identityService.Logout();
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
