using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using UI_Interface.Contracts.Services;
using UI_Interface.Helpers;
using UI_Interface.Properties;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    public class LogInViewModel : ObservableObject
    {
        private readonly IIdentityService _identityService;
        private string _statusMessage;
        private bool _isBusy;

        public LogInViewModel(IIdentityService identityService)
        {
            _identityService = identityService;
            LoginCommand = new RelayCommand(OnLogin, () => !IsBusy);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _ = SetProperty(ref _isBusy, value);
                LoginCommand.NotifyCanExecuteChanged();
            }
        }

        public RelayCommand LoginCommand { get; }

        private static string GetStatusMessage(LoginResultType loginResult)
        {
            return loginResult switch
            {
                LoginResultType.Unauthorized => Resources.StatusUnauthorized,
                LoginResultType.UnknownError => Resources.StatusUnknownError,
                LoginResultType.NotConnectionToDb => Resources.StatusNotConnectionToDb,
                LoginResultType.Success or LoginResultType.CancelledByUser => string.Empty,
                _ => string.Empty,
            };
        }

        private async void OnLogin()
        {
            IsBusy = true;
            StatusMessage = string.Empty;
            LoginResultType loginResult = await _identityService.LoginAsync();
            StatusMessage = GetStatusMessage(loginResult);
            IsBusy = false;
        }
    }
}
