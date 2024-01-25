using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BLL.Contracts;
using DAL.Enums;
using DAL.Models;
using DAL.Models.Master;
using DocumentFormat.OpenXml.Drawing;

using MahApps.Metro.Controls.Dialogs;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NavigationManager.Frame.Extension.WPF;
using UI_Interface.Models;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    public class DeliveryPageViewModel : ObservableObject, INavigationAware
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        private readonly INavigationManager _navigationManager;

        private readonly IProcessService _processService;

        private readonly IUserService _userService;

        public DeliveryPageViewModel(INavigationManager navigationManager, IProcessService processService, IUserService userService, IDialogCoordinator dialogCoordinator)
        {
            _navigationManager = navigationManager;

            _processService = processService;

            _userService = userService;

            _dialogCoordinator = dialogCoordinator;

            UploadFileCommand = new AsyncRelayCommand(UploadFile);
        }

        public AsyncRelayCommand UploadFileCommand { get; }

        public void OnNavigatedFrom()
        {
            
        }

        public void OnNavigatedTo(object parameter)
        {
            if (parameter is List<StepViewModel> stepCollection)
            {
                stepCollection = stepCollection.OrderBy(collection => collection.ProcessStep.Step).ToList();

                foreach (StepViewModel step in stepCollection)
                {
                    if (step.ProcessStep.Process.ProcessName == AppProcess.UploadInvoices)
                    {
                        NavigateToValidationRowData(step.ProcessStep);
                    }
                }
            }
        }

        private void NavigateToValidationRowData(ProcessStep processStep)
        {
            _ = _navigationManager.NavigateTo(
                typeof(FileValidationViewModel).FullName,
                processStep);
        }

        private async Task UploadFile()
        {
            User user = _userService.GetCurrentUser();

            if (user is null)
            {
                _ = _dialogCoordinator.ShowModalMessageExternal(this, $"Ошибка", "Пользователь не определен, попробуйте перезапустить в приложение.");

                return;
            }

            IEnumerable<ProcessStep> processSteps;

            try
            {
                processSteps = await _processService.GetProcessStepsByUserSectionAsync(user.SectionId, AppProcess.UploadInvoices);

                if (processSteps is null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                _ = _dialogCoordinator.ShowModalMessageExternal(this, $"Ошибка получения шагов для загрузки файла", ex.Message);

                return;
            }

            MasterTransfer masterTransfer = new()
            {
                Object = processSteps,
                ViewModel = this
            };

            _ = _navigationManager.NavigateTo(
                typeof(MasterViewModel).FullName,
                masterTransfer);
        }
    }
}
