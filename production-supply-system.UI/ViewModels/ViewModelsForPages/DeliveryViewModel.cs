using DAL.Models;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using Microsoft.Toolkit.Mvvm.Input;
using NavigationManager.Frame.Extension.WPF;
using System.Threading.Tasks;
using System.Windows.Media;
using UI_Interface.Properties;
using System.Windows;
using System.Linq;
using BLL.Contracts;
using DAL.Models.Master;
using System.Collections.Generic;
using BLL.Models;
using System.Collections.ObjectModel;
using UI_Interface.Extensions;
using System;
using Microsoft.Extensions.Logging;
using DAL.Enums;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    public class DeliveryViewModel : ControlledViewModel, INavigationAware
    {
        private readonly ILogger<DeliveryViewModel> _logger;

        private readonly INavigationManager _navigationManager;

        private readonly IStaticDataService _staticDataService;

        private readonly IDeliveryService _deliveryService;

        private readonly IDocumentService _documentService;

        private readonly IExcelService _excelService;

        private DeliveryDetailViewModel _deliveryDetailViewModel;

        private bool _hasErrors;

        private List<ProcessStep> _processSteps;

        private bool _detailsOpen;

        private bool _isLotListEmpty;

        private bool _isProcessStepsShow;

        private ObservableCollection<StepViewModel> _masterCollection;

        private bool _hasErrorsInDetails;

        private bool _hasErrorsInCollection;

        private ObservableCollection<LotViewModel> _lots;

        public DeliveryViewModel(
            ILogger<DeliveryViewModel> logger,
            INavigationManager navigationManager, 
            IStaticDataService staticDataService, 
            IDeliveryService deliveryService, 
            IDocumentService documentService, 
            IExcelService excelService)
        {
            _logger = logger;

            _navigationManager = navigationManager;

            _staticDataService = staticDataService;

            _deliveryService = deliveryService;

            _documentService = documentService;

            _excelService = excelService;

            NavigateToDetailsCommand = new RelayCommand<Lot>(NavigateToDetails);
            OpenDetailsCommand = new AsyncRelayCommand(OpenDetailsFlyoutAsync);
            CloseDetailsCommand = new RelayCommand(CloseDetailsFlyout);
            StartProcessCommand = new AsyncRelayCommand(StartProcessAsync);
            GetLotsCommand = new AsyncRelayCommand(GetAllLotItemsAsync);
            ExportAllTracingCommand = new AsyncRelayCommand(ExportAllTracing);

            _metroWindow = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault(x => x.IsActive);
        }

        /// <summary>
        /// Команда перенаправления на страницу с детальной информацией лота
        /// </summary>
        public RelayCommand<Lot> NavigateToDetailsCommand { get; }

        /// <summary>
        /// Асинхронная команда открытия панели с детальной информацией
        /// </summary>
        public AsyncRelayCommand OpenDetailsCommand { get; }

        /// <summary>
        /// Команда закрытия панели с детальной информацией
        /// </summary>
        public RelayCommand CloseDetailsCommand { get; }

        /// <summary>
        /// Асинхронная команда начала загрузки лота
        /// </summary>
        public AsyncRelayCommand StartProcessCommand { get; }

        /// <summary>
        /// Асинхронная команда получения списка лотов
        /// </summary>
        public AsyncRelayCommand GetLotsCommand { get; }

        /// <summary>
        /// Асинхронная команда выгрузки файла c трейсингом
        /// </summary>
        public AsyncRelayCommand ExportAllTracingCommand { get; }

        /// <summary>
        /// Получает или задаёт коллекцию лотов
        /// </summary>
        public ObservableCollection<LotViewModel> Lots
        {
            get => _lots;
            set => _ = SetProperty(ref _lots, value);
        }

        /// <summary>
        /// Флаг указывающий открыта ли панель с детальной информацией
        /// </summary>
        public bool DetailsOpen
        {
            get => _detailsOpen;
            set => _ = SetProperty(ref _detailsOpen, value);
        }

        /// <summary>
        /// Флаг указывающий, пустой ли список лотов
        /// </summary>
        public bool IsLotListEmpty
        {
            get => _isLotListEmpty;
            set => _ = SetProperty(ref _isLotListEmpty, value);
        }

        /// <summary>
        /// Флаг указывающий, нужно ли отображать шаги
        /// </summary>
        public bool IsProcessStepsShow
        {
            get => _isProcessStepsShow;
            set => _ = SetProperty(ref _isProcessStepsShow, value);
        }

        /// <summary>
        /// Получает или задаёт модель преддставления детальной информации
        /// </summary>
        public DeliveryDetailViewModel DeliveryDetailViewModel
        {
            get => _deliveryDetailViewModel;
            set => _ = SetProperty(ref _deliveryDetailViewModel, value);
        }

        /// <summary>
        /// Флаг, указывающий, есть ли ошибки во всех валидируемых полях.
        /// </summary>
        public bool HasErrors
        {
            get => _hasErrors;
            set => _ = SetProperty(ref _hasErrors, value);
        }

        /// <summary>
        /// Коллекция шагов процесса, связанная с пользовательским интерфейсом.
        /// </summary>
        public ObservableCollection<StepViewModel> MasterCollection
        {
            get => _masterCollection;
            set => _ = SetProperty(ref _masterCollection, value);
        }

        /// <summary>
        /// Флаг, указывающий, есть ли ошибки в коллекции шагов.
        /// </summary>
        public bool HasErrorsInCollection
        {
            get => _hasErrorsInCollection = HasErrorsInCollectionProcesses();
            set => _ = SetProperty(ref _hasErrorsInCollection, value);
        }

        /// <summary>
        /// Флаг, указывающий, есть ли ошибки в детальной информации.
        /// </summary>
        public bool HasErrorsInDetails
        {
            get => _hasErrorsInDetails;
            set => _ = SetProperty(ref _hasErrorsInDetails, value);
        }

        /// <summary>
        /// Преобразование view models в models
        /// </summary>
        /// <param name="stepCollection">Список view models</param>
        /// <returns></returns>
        private List<ProcessStep> AdaptStepsToService(List<StepViewModel> stepCollection)
        {
            _logger.LogInformation("The beginning of adapting view models to models list.");

            List<ProcessStep> steps = new();

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

            _logger.LogInformation("The adaptation of view models to models is completed.");
            
            return steps;
        }

        public void OnNavigatedFrom()
        {
            _processSteps = null;
        }

        public void OnNavigatedTo(object parameter)
        {
            Lots = new();

            DetailsOpen = false;

            IsLotListEmpty = Lots.Count == 0;

            HasErrors = true;

            HasErrorsInDetails = true;
        }

        /// <summary>
        /// Получает список лотов
        /// </summary>
        private async Task GetAllLotItemsAsync()
        {
            try
            {
                _logger.LogInformation("The beginning of receiving lots.");

                 await CreateController(Resources.BllDownloadLots);

                Lots.Clear();

                foreach (Lot lot in await _deliveryService.GetAllLotsAsync())
                {
                    int quantityContainers = await _deliveryService.GetquantityContainersForLotId(lot.Id);

                    LotViewModel lotViewModel = new(lot, quantityContainers);

                    Lots.Add(lotViewModel);
                }

                IsLotListEmpty = Lots.Count == 0;

                _logger.LogInformation("The receipt of lots is completed.");
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
        /// Обработчик события, вызываемого при обновлении данных в объекте StepViewModel.
        /// </summary>
        private void OnStepViewModelUpdated(object sender, StepViewModel stepViewModel)
        {
            HasErrorsInCollection = HasErrorsInCollectionProcesses();

            HasErrors = HasErrorsInDetails || HasErrorsInCollection;
        }

        /// <summary>
        /// Закрытие панери с детальной информацией
        /// </summary>
        private void CloseDetailsFlyout()
        {
            DeliveryDetailViewModel = null;

            HasErrors = true;

            DetailsOpen = !DetailsOpen;
        }

        /// <summary>
        /// Формирует обьект лота и перенаправляет на страницу редактирования лота
        /// </summary>
        /// <returns>Задача представляющая асинхронную операцию</returns>
        private async Task StartProcessAsync()
        {
            try
            {
                _logger.LogInformation("The beginning of lot formation.");

                await CreateController(Resources.BllUploadLot);

                _deliveryService.DeliveryLoadProgressUpdated += OnDeliveryLoadProgressUpdated;

                Lot lot = await _deliveryService.StartProcessAsync(AdaptStepsToService(MasterCollection.ToList()), DeliveryDetailViewModel.Lot);

                await WaitForMessageUnlock(Resources.BllUploadLot, Resources.ShellUploadSuccess, Brushes.Green);

                NavigateToDetails(lot);
            }
            catch (Exception ex)
            {
                await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);
            }
            finally
            {
                await ControllerPostProcess();

                _deliveryService.DeliveryLoadProgressUpdated -= OnDeliveryLoadProgressUpdated;
            }
        }

        /// <summary>
        /// Перенаправляет на страницу редактирования лота
        /// </summary>
        /// <param name="lot">Лот</param>
        private void NavigateToDetails(Lot lot)
        {
            if (lot is not null)
            {
                _logger.LogInformation($"A lot with a unique id '{lot.Id}' has been formed, and redirects to the lot editing page.");

                _ = _navigationManager.NavigateTo(typeof(EditDeliveryViewModel).FullName, lot.Id);

                _logger.LogInformation($"Redirecting to the lot editing page completed.");
            }
        }

        private async Task ExportAllTracing()
        {
            try
            {
                CommonOpenFileDialog dialog = new();

                dialog.IsFolderPicker = true;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    await CreateController(Resources.BllExportAllTracing);

                    _logger.LogInformation("Starting export all tracing to file.");

                    await _deliveryService.ExportAllTracing(dialog.FileName);

                    _logger.LogInformation("Export all tracing to file completed.");
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
        private async Task OpenDetailsFlyoutAsync()
        {
            DetailsOpen = true;

            MasterCollection = new();

            try
            {
                if (DeliveryDetailViewModel is null)
                {
                    List<Type> validatedModels = new()
                    {
                        typeof(Lot),
                        typeof(Shipper),
                        typeof(Transport),
                        typeof(Invoice)
                    };

                    await CreateController(Resources.BllLoadStaticData);

                    DeliveryDetailViewModel = new DeliveryDetailViewModel(validatedModels, _staticDataService, _deliveryService, _logger, _progressController);

                    DeliveryDetailViewModel.HasErrorsUpdated += OnHasErrorsUpdated;
                }

                await CreateController(Resources.BllGetProcessSteps);

                _logger.LogInformation($"The beginning of receiving the steps of operations '{AppProcess.UploadInvoicesPartner2}' for the current user by his section.");

                _processSteps = await _deliveryService.GetProcessStepsByUserSectionAsync(AppProcess.UploadInvoicesPartner2);

                _logger.LogInformation($"Getting the steps of operations '{AppProcess.UploadInvoicesPartner2}' for the current user by his section is completed.");

                if (_processSteps.Count == 0)
                {
                    await WaitForMessageUnlock(Resources.BllGetProcessSteps, Resources.ShellProcessForbidden, Brushes.IndianRed);

                    _logger.LogWarning($"Access denied.");

                    return;
                }

                foreach (ProcessStep masterItem in _processSteps.OrderBy(s => s.Step))
                {
                    StepViewModel stepViewModel = new(_excelService, _documentService, _logger, masterItem);

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
                bool hasShipperNotSelected = DeliveryDetailViewModel.Lot.Shipper is null;
                bool hasCarrierNotSelected = DeliveryDetailViewModel.Lot.Carrier is null;
                bool hasDeliveryTermNotSelected = DeliveryDetailViewModel.Lot.DeliveryTerms is null;
                bool hasTransportTypeNotSelected = DeliveryDetailViewModel.Lot.LotTransportType is null;
                bool hasArrivalLocationNotSelected = DeliveryDetailViewModel.Lot.LotArrivalLocation is null;
                bool hasDepartureLocationNotSelected = DeliveryDetailViewModel.Lot.LotDepartureLocation is null;
                bool hasEtaNotSelected = DeliveryDetailViewModel.Lot.LotEta is null;
                bool hasPurchaseOrderNotSelected = DeliveryDetailViewModel.Lot.LotPurchaseOrder is null;
                bool hasTransportNameNotSelected = DeliveryDetailViewModel.Lot.LotTransport.TransportName is null;
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

        /// <summary>
        /// Проверяет, есть ли ошибки в коллекции шагов.
        /// </summary>
        /// <returns>True, если есть ошибки, иначе False.</returns>
        private bool HasErrorsInCollectionProcesses()
        {
            return !MasterCollection.All(item => item.HasError is false);
        }

        /// <summary>
        /// Обработчик события, вызываемого при наличии ошибки в объекте StepViewModel.
        /// </summary>
    }
}
