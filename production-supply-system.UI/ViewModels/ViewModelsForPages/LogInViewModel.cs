using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UI_Interface.Contracts.Services;
using UI_Interface.Helpers;
using UI_Interface.Multilang;
using UI_Interface.Properties;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    public partial class LogInViewModel : ObservableObject
    {
        private readonly IIdentityService _identityService;

        [ObservableProperty]
        private string _statusMessage;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private bool _isBusy;

        public LogInViewModel(IIdentityService identityService, IMultilangManager multilangManager)
        {
            _identityService = identityService;

            multilangManager.InitializeLanguage();
        }

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

        private bool CanLogin()
        {
            return !IsBusy;
        }

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task LoginAsync()
        {
            IsBusy = true;
            StatusMessage = string.Empty;
            LoginResultType loginResult = await _identityService.LoginAsync();
            StatusMessage = GetStatusMessage(loginResult);
            IsBusy = false;
        }
    }
}
