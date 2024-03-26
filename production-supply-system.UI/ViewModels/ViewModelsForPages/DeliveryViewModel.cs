using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

using BLL.Contracts;
using BLL.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using NavigationManager.Frame.Extension.WPF;

using production_supply_system.EntityFramework.DAL.Enums;
using production_supply_system.EntityFramework.DAL.LotContext.Models;
using production_supply_system.EntityFramework.DAL.MasterProcessContext.Models;

using UI_Interface.Contracts;
using UI_Interface.Properties;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    public partial class DeliveryViewModel(
            ILogger<DeliveryViewModel> logger,
            INavigationManager navigationManager,
            IStaticDataService staticDataService,
            IDeliveryService deliveryService,
            IExportProceduresService exportProcedures,
            IToastNotificationsService toastNotificationsService,
            IExcelService excelService) : ControlledViewModel(logger), INavigationAware
    {
        [ObservableProperty]
        private bool _hasErrors;

        [ObservableProperty]
        private bool _detailsOpen;

        [ObservableProperty]
        private bool _exportOpen;

        [ObservableProperty]
        private bool _isLotListEmpty;

        [ObservableProperty]
        private bool _isProcessStepsShow;

        [ObservableProperty]
        private bool _hasErrorsInDetails;

        [ObservableProperty]
        private bool _hasErrorsInCollection;

        [ObservableProperty]
        private DeliveryDetailViewModel _deliveryDetailViewModel;

        [ObservableProperty]
        private List<ProcessesStep> _processSteps;

        [ObservableProperty]
        private ObservableCollection<StepViewModel> _masterCollection;

        [ObservableProperty]
        private ObservableCollection<LotViewModel> _lots;

        /// <summary>
        /// Преобразование view models в models
        /// </summary>
        /// <param name="stepCollection">Список view models</param>
        /// <returns></returns>
        private static List<ProcessesStep> AdaptStepsToService(List<StepViewModel> stepCollection)
        {
            List<ProcessesStep> steps = [];

            foreach (StepViewModel stepViewModel in stepCollection)
            {
                steps.Add(new()
                {
                    Id = stepViewModel.ProcessStep.Id,
                    Docmapper = stepViewModel.ProcessStep.Docmapper,
                    DocmapperId = stepViewModel.ProcessStep.DocmapperId,
                    Process = stepViewModel.ProcessStep.Process,
                    ProcessId = stepViewModel.ProcessStep.ProcessId,
                    Section = stepViewModel.ProcessStep.Section,
                    SectionId = stepViewModel.ProcessStep.SectionId,
                    Step = stepViewModel.ProcessStep.Step,
                    StepName = stepViewModel.ProcessStep.StepName
                });
            }

            return steps;
        }

        /// <summary>
        /// Проверяет, есть ли ошибки в коллекции шагов.
        /// </summary>
        /// <returns>True, если есть ошибки, иначе False.</returns>
        private bool HasErrorsInCollectionProcesses()
        {
            return !MasterCollection.All(item => item.HasError is false);
        }

        public void OnNavigatedFrom()
        {
            ProcessSteps = null;
        }

        public async void OnNavigatedTo(object parameter)
        {
            Lots = [];

            DetailsOpen = false;

            IsLotListEmpty = Lots.Count == 0;

            HasErrors = true;

            HasErrorsInDetails = true;

            await GetLotsAsync();
        }

        /// <summary>
        /// Получает список лотов
        /// </summary>
        [RelayCommand]
        private async Task GetLotsAsync()
        {
            try
            {
                logger.LogInformation(Resources.LogDownloadLots);

                await CreateController(Resources.BllDownloadLots);

                Lots.Clear();

                foreach (Lot lot in await deliveryService.GetAllLotsAsync())
                {
                    Lots.Add(new(lot));
                }

                IsLotListEmpty = Lots.Count == 0;

                logger.LogInformation($"{Resources.LogDownloadLots} {Resources.Completed}");
            }
            catch (Exception ex)
            {
                await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);
            }
            finally
            {
                await ControllerPostProcess();
            }
        }

        /// <summary>
        /// Закрытие панери с детальной информацией
        /// </summary>
        [RelayCommand]
        private void CloseDetails()
        {
            DeliveryDetailViewModel = null;

            HasErrors = true;

            DetailsOpen = !DetailsOpen;
        }

        /// <summary>
        /// Закрытие панели с выгрузкой файлов
        /// </summary>
        [RelayCommand]
        private void CloseExport()
        {
            ExportOpen = !ExportOpen;
        }

        /// <summary>
        /// Формирует обьект лота и перенаправляет на страницу редактирования лота
        /// </summary>
        [RelayCommand]
        private async Task StartUploadLotAsync()
        {
            try
            {
                if (DeliveryDetailViewModel is not null)
                {
                    logger.LogInformation(Resources.LogUploadLot);

                    await CreateController(Resources.BllUploadLot);

                    Lot details = new()
                    {
                        LotNumber = DeliveryDetailViewModel.LotNumber,
                        ShipperId = DeliveryDetailViewModel.Shipper.Id,
                        LotPurchaseOrderId = DeliveryDetailViewModel.LotPurchaseOrder.Id,
                        CarrierId = DeliveryDetailViewModel.Carrier.Id,
                        DeliveryTermsId = DeliveryDetailViewModel.DeliveryTerms.Id,
                        LotTransportId = DeliveryDetailViewModel.LotTransport?.Id,
                        LotTransportTypeId = DeliveryDetailViewModel.LotTransportType.Id,
                        LotTransportDocument = DeliveryDetailViewModel.LotTransportDocument,
                        LotEtd = DeliveryDetailViewModel.LotEtd,
                        LotAtd = DeliveryDetailViewModel.LotAtd,
                        LotEta = (DateTime)DeliveryDetailViewModel.LotEta,
                        LotAta = DeliveryDetailViewModel.LotAta,
                        LotDepartureLocationId = DeliveryDetailViewModel.LotDepartureLocation.Id,
                        LotCustomsLocationId = DeliveryDetailViewModel.LotCustomsLocation?.Id,
                        LotArrivalLocationId = DeliveryDetailViewModel.LotArrivalLocation.Id,
                        CloseDate = DeliveryDetailViewModel.CloseDate,
                        LotComment = DeliveryDetailViewModel.LotComment,
                        Carrier = DeliveryDetailViewModel.Carrier,
                        DeliveryTerms = DeliveryDetailViewModel.DeliveryTerms,
                        LotDepartureLocation = DeliveryDetailViewModel.LotDepartureLocation,
                        LotCustomsLocation = DeliveryDetailViewModel.LotCustomsLocation,
                        LotArrivalLocation = DeliveryDetailViewModel.LotArrivalLocation,
                        LotPurchaseOrder = DeliveryDetailViewModel.LotPurchaseOrder,
                        LotTransport = DeliveryDetailViewModel.LotTransport,
                        LotTransportType = DeliveryDetailViewModel.LotTransportType,
                        Shipper = DeliveryDetailViewModel.Shipper
                    };

                    deliveryService.DeliveryLoadProgressUpdated += OnDeliveryLoadProgressUpdated;

                    Lot lot = await deliveryService.StartUploadLotAsync(AdaptStepsToService([.. MasterCollection]), details);

                    await WaitForMessageUnlock(Resources.BllUploadLot, Resources.ShellUploadSuccess, Brushes.Green);

                    logger.LogInformation(string.Format(Resources.LogUploadLotCompleted, lot.Id));

                    NavigateToDetails(lot);
                }
            }
            catch (Exception ex)
            {
                await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);
            }
            finally
            {
                await ControllerPostProcess();

                deliveryService.DeliveryLoadProgressUpdated -= OnDeliveryLoadProgressUpdated;
            }
        }

        /// <summary>
        /// Перенаправляет на страницу редактирования лота
        /// </summary>
        /// <param name="lot">Лот</param>
        [RelayCommand]
        private void NavigateToDetails(Lot lot)
        {
            if (lot is not null)
            {
                logger.LogInformation(Resources.LogRedirectToEditingLot);

                _ = navigationManager.NavigateTo(typeof(EditDeliveryViewModel).FullName, lot.Id);

                logger.LogInformation($"{Resources.LogRedirectToEditingLot} {Resources.Completed}");
            }
        }

        [RelayCommand]
        private async Task OpenExportAsync()
        {
            ExportOpen = true;

            try
            {
                MasterCollection = [];

                await CreateController(Resources.ShellExportFiles);

                ProcessSteps = await GetProcessStepsAsync(AppProcess.ExportFileToExcelPartner2);

                if (ProcessSteps.Count == 0)
                {
                    await WaitForMessageUnlock(Resources.BllGetProcessSteps, Resources.ShellProcessForbidden, Brushes.IndianRed);

                    logger.LogWarning(Resources.LogAccessDenied);

                    return;
                }

                foreach (ProcessesStep masterItem in ProcessSteps)
                {
                    StepViewModel stepViewModel = new(excelService, logger, exportProcedures, toastNotificationsService, masterItem);

                    stepViewModel.HasStepViewModelUpdated += OnStepViewModelUpdated;

                    MasterCollection.Add(stepViewModel);
                }
            }
            catch (Exception ex)
            {
                await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);

                return;
            }
            finally
            {
                await ControllerPostProcess();
            }
        }

        /// <summary>
        /// Открытие панели с детальной информацией
        /// </summary>
        [RelayCommand]
        private async Task OpenDetailsAsync()
        {
            DetailsOpen = true;

            MasterCollection = [];

            try
            {
                if (DeliveryDetailViewModel is null)
                {
                    List<Type> validatedModels =
                    [
                        typeof(Lot),
                        typeof(Shipper),
                        typeof(Transport),
                        typeof(Invoice)
                    ];

                    await CreateController(Resources.BllLoadStaticData);

                    DeliveryDetailViewModel = new DeliveryDetailViewModel(validatedModels, staticDataService, deliveryService, logger, _progressController);

                    DeliveryDetailViewModel.HasErrorsUpdated += OnHasErrorsUpdated;
                }

                await CreateController(Resources.BllGetProcessSteps);

                ProcessSteps = await GetProcessStepsAsync(AppProcess.UploadInvoicesPartner2);

                if (ProcessSteps.Count == 0)
                {
                    await WaitForMessageUnlock(Resources.BllGetProcessSteps, Resources.ShellProcessForbidden, Brushes.IndianRed);

                    logger.LogWarning(Resources.LogAccessDenied);

                    return;
                }

                foreach (ProcessesStep masterItem in ProcessSteps)
                {
                    StepViewModel stepViewModel = new(excelService, logger, exportProcedures, toastNotificationsService, masterItem);

                    stepViewModel.HasStepViewModelUpdated += OnStepViewModelUpdated;

                    MasterCollection.Add(stepViewModel);
                }
            }
            catch (Exception ex)
            {
                await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);

                return;
            }
            finally
            {
                await ControllerPostProcess();
            }
        }

        /// <summary>
        /// Обработчик события, вызываемого при обновлении данных в объекте StepViewModel.
        /// </summary>
        private void OnStepViewModelUpdated(object sender, StepViewModel stepViewModel)
        {
            HasErrorsInCollection = HasErrorsInCollectionProcesses();

            HasErrors = HasErrorsInDetails || HasErrorsInCollection;
        }

        /// <summary>
        /// Обработчик события, обновления состояния прогресса
        /// </summary>
        private void OnDeliveryLoadProgressUpdated(object sender, ControllerDetails details)
        {
            if (details.ProgressValue is not null)
            {
                _progressController.SetProgress((double)details.ProgressValue);
            }

            _progressController.SetTitle(details.Title);

            _progressController.SetMessage(details.Message);
        }

        /// <summary>
        /// Обработчик события, срабатываемый при обновлении состояния ошибок в детальной информации
        /// </summary>
        /// <param name="_"></param>
        /// <param name="result"></param>
        private void OnHasErrorsUpdated(object _, bool result)
        {
            HasErrorsInDetails = HasErrorsInLotDetails(result);

            HasErrorsInCollection = HasErrorsInCollectionProcesses();

            HasErrors = HasErrorsInDetails || HasErrorsInCollection;
        }

        /// <summary>
        /// Определяет есть ли ошибки в детальной информации
        /// </summary>
        /// <param name="result">значение наличия ошибок в детальной информации</param>
        private bool HasErrorsInLotDetails(bool result)
        {
            if (DeliveryDetailViewModel is not null)
            {
                bool hasErrorsInDetails = DeliveryDetailViewModel.HasErrors;
                bool hasShipperNotSelected = DeliveryDetailViewModel.Shipper is null;
                bool hasCarrierNotSelected = DeliveryDetailViewModel.Carrier is null;
                bool hasDeliveryTermNotSelected = DeliveryDetailViewModel.DeliveryTerms is null;
                bool hasTransportTypeNotSelected = DeliveryDetailViewModel.LotTransportType is null;
                bool hasArrivalLocationNotSelected = DeliveryDetailViewModel.LotArrivalLocation is null;
                bool hasDepartureLocationNotSelected = DeliveryDetailViewModel.LotDepartureLocation is null;
                bool hasEtaNotSelected = DeliveryDetailViewModel.LotEta is null;
                bool hasPurchaseOrderNotSelected = DeliveryDetailViewModel.LotPurchaseOrder is null;
                bool hasTransportNameNotSelected = DeliveryDetailViewModel.LotTransport.TransportName is null;
                bool hasTransportNameNotAdded = DeliveryDetailViewModel.IsAddTransportVisible;

                return
                    hasErrorsInDetails ||
                    hasShipperNotSelected ||
                    hasCarrierNotSelected ||
                    hasDeliveryTermNotSelected ||
                    hasTransportTypeNotSelected ||
                    hasArrivalLocationNotSelected ||
                    hasDepartureLocationNotSelected ||
                    hasEtaNotSelected ||
                    hasPurchaseOrderNotSelected ||
                    hasTransportNameNotSelected ||
                    hasTransportNameNotAdded ||
                    result;
            }

            return true;
        }

        private async Task<List<ProcessesStep>> GetProcessStepsAsync(AppProcess appProcess)
        {
            logger.LogInformation(Resources.LogProcessStepsGet);

            List<ProcessesStep> result = await deliveryService.GetProcessStepsByUserSectionAsync(appProcess);

            logger.LogInformation($"{string.Format(Resources.LogProcessStepsGet, appProcess)} {Resources.Completed}");

            return result;
        }
    }
}
