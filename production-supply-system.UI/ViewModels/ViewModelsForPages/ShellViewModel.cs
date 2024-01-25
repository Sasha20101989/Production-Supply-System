using System;
using System.Collections.ObjectModel;
using System.Linq;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NavigationManager.Frame.Extension.WPF;
using UI_Interface.Contracts.Services;
using UI_Interface.Properties;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    public class ShellViewModel : ObservableObject
    {
        private readonly INavigationManager _navigationManager;

        private readonly IUserDataService _userDataService;

        private HamburgerMenuItem _selectedMenuItem;

        private HamburgerMenuItem _selectedOptionsMenuItem;

        public ShellViewModel(INavigationManager navigationManager, IUserDataService userDataService)
        {
            _navigationManager = navigationManager;
            _userDataService = userDataService;

            GoBackCommand = new RelayCommand(OnGoBack, CanGoBack);
            MenuItemInvokedCommand = new RelayCommand(OnMenuItemInvoked);
            OptionsMenuItemInvokedCommand = new RelayCommand(OnOptionsMenuItemInvoked);
            LoadedCommand = new RelayCommand(OnLoaded);
            UnloadedCommand = new RelayCommand(OnUnloaded);
        }

        public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
        {
            new HamburgerMenuIconItem() { Label = Resources.ShellMainPage, Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.HomeSolid }, TargetPageType = typeof(MainViewModel) },
            new HamburgerMenuIconItem() { Label = Resources.ShellDocumentMapperPage, Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.MapMarkerSolid }, TargetPageType = typeof(DocumentMapperViewModel) },
            new HamburgerMenuIconItem() { Label = Resources.ShellDeliveryPage, Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.CalendarAltSolid }, TargetPageType = typeof(DeliveryPageViewModel) }
        };

        public HamburgerMenuItem SelectedMenuItem
        {
            get => _selectedMenuItem;
            set => _ = SetProperty(ref _selectedMenuItem, value);
        }

        public HamburgerMenuItem SelectedOptionsMenuItem
        {
            get => _selectedOptionsMenuItem;
            set => _ = SetProperty(ref _selectedOptionsMenuItem, value);
        }

        public ObservableCollection<HamburgerMenuItem> OptionMenuItems { get; set; } = new();

        public RelayCommand GoBackCommand { get; }

        public RelayCommand MenuItemInvokedCommand { get; }

        public RelayCommand OptionsMenuItemInvokedCommand { get; }

        public RelayCommand LoadedCommand { get; }

        public RelayCommand UnloadedCommand { get; }

        private void OnUserItemSelected()
        {
            NavigateTo(typeof(SettingsViewModel));
        }

        private void OnUserDataUpdated(object sender, UserViewModel user)
        {
            HamburgerMenuImageItem userMenuItem = OptionMenuItems.OfType<HamburgerMenuImageItem>().FirstOrDefault();
            if (userMenuItem is not null && user is not null)
            {
                userMenuItem.Label = user.Name;
                userMenuItem.Thumbnail = user.Photo;
            }
        }

        private void OnGoBack()
        {
            _navigationManager.GoBack();
        }

        private bool CanGoBack()
        {
            return _navigationManager.CanGoBack;
        }

        private void OnMenuItemInvoked()
        {
            NavigateTo(SelectedMenuItem.TargetPageType);
        }

        private void OnOptionsMenuItemInvoked()
        {
            NavigateTo(SelectedOptionsMenuItem.TargetPageType);
        }

        private void OnLoaded()
        {
            _navigationManager.Navigated += OnNavigated;

            _userDataService.UserDataUpdated += OnUserDataUpdated;

            UserViewModel user = _userDataService.GetUser();

            HamburgerMenuImageItem userMenuItem = new()
            {
                Thumbnail = user.Photo,
                Label = user.Name,
                Command = new RelayCommand(OnUserItemSelected)
            };

            OptionMenuItems.Insert(0, userMenuItem);
        }

        private void OnUnloaded()
        {
            _navigationManager.Navigated -= OnNavigated;

            _userDataService.UserDataUpdated -= OnUserDataUpdated;

            MenuItems.Add(new HamburgerMenuIconItem() { Label = Resources.MainPageTitle, Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.HomeSolid }, TargetPageType = typeof(MainViewModel) });
        }

        private void OnNavigated(object sender, string viewModelName)
        {
            HamburgerMenuItem item = MenuItems
                        .OfType<HamburgerMenuItem>()
                        .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);

            if (item is not null)
            {
                SelectedMenuItem = item;
            }
            else
            {
                SelectedOptionsMenuItem = OptionMenuItems
                        .OfType<HamburgerMenuItem>()
                        .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
            }

            GoBackCommand.NotifyCanExecuteChanged();
        }

        private void NavigateTo(Type targetViewModel)
        {
            if (targetViewModel is not null)
            {
                _ = _navigationManager.NavigateTo(targetViewModel.FullName);
            }
        }
    }
}
