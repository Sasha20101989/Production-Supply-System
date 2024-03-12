using System;
using System.Threading.Tasks;
using BLL.Contracts;
using DAL.Models;
using MahApps.Metro.Controls;
using NavigationManager.Frame.Extension.WPF;
using System.Windows.Media;
using UI_Interface.Properties;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using UI_Interface.Contracts;
using UI_Interface.Filters;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    /// <summary>
    /// ViewModel, представляющая информацию о лоте для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ControlledViewModel для уведомлений об изменении свойств и выдачи уведомлений.
    /// </summary>
    public class EditDeliveryViewModel : ControlledViewModel, INavigationAware
    {
        private readonly ILogger<EditDeliveryViewModel> _logger;

        private readonly IDeliveryService _deliveryService;

        private readonly IStaticDataService _staticDataService;

        private DeliveryDetailViewModel _deliveryDetailViewModel;

        private ObservableCollection<PartsInContainer> _parts;

        private ObservableCollection<ContainersInLot> _containers;

        private ContainersInLot _selectedContainer;

        private string _filterCaseNumber;
        private string _filterInvoiceNumber;
        private string _filterPartNameEng;
        private string _filterQuantity;
        private string _filterPartNumber;
        private string _filterSupplierPackingType;
        private string _filterNetWeight;
        private string _filterGrossWeight;
        private string _filterContainerNumber;
        private string _filterContainerType;
        private string _filterSealNumber;
        private string _filterIMO;
        private string _filterContainerComment;

        private string _titleParts;

        private ICollectionView _partsView;
        private ICollectionView _containersView;

        private bool _detailsOpen;

        private List<ICustomFilter> _partsFilters;
        private List<ICustomFilter> _containersFilters;

        public EditDeliveryViewModel(IStaticDataService staticDataService, IDeliveryService deliveryService, ILogger<EditDeliveryViewModel> logger)
        {
            _logger = logger;

            _staticDataService = staticDataService;

            _deliveryService = deliveryService;

            _metroWindow = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault(x => x.IsActive);

            OpenDetailsCommand = new RelayCommand(OpenDetailsFlyout);
            CloseDetailsCommand = new RelayCommand(CloseDetailsFlyout);
            EditDetailsCommand = new AsyncRelayCommand(EditDetailsAsync);

            Parts = new();
            Containers = new();

            LotLoaded += OnLotLoadedAsync;
            ContainersLoaded += OnContainersLoaded;

            DetailsOpen = false;

            TitleParts = Resources.TitlePartsForContainer;
        }

        /// <summary>
        /// Асинхронная команда открытия панели с детальной информацией
        /// </summary>
        public RelayCommand OpenDetailsCommand { get; }

        /// <summary>
        /// Команда закрытия панели с детальной информацией
        /// </summary>
        public RelayCommand CloseDetailsCommand { get; }

        /// <summary>
        /// Команда редактирования детальной информации
        /// </summary>
        public AsyncRelayCommand EditDetailsCommand { get; }

        /// <summary>
        /// Заголовок раздела с деталями
        /// </summary>
        public string TitleParts
        {
            get => _titleParts;
            set => _ = SetProperty(ref _titleParts, value);
        }

        /// <summary>
        /// Флаг указывающий, открыта ли панель с детальной информацией
        /// </summary>
        public bool DetailsOpen
        {
            get => _detailsOpen;
            set => _ = SetProperty(ref _detailsOpen, value);
        }

        /// <summary>
        /// Коллекция деталей
        /// </summary>
        public ICollectionView PartsView
        {
            get => _partsView;
            set => _ = SetProperty(ref _partsView, value);
        }

        /// <summary>
        /// Коллекция контейнеров
        /// </summary>
        public ICollectionView ContainersView
        {
            get => _containersView;
            set => _ = SetProperty(ref _containersView, value);
        }

        /// <summary>
        /// Устанавливает или получает модель представления с детальной информацией
        /// </summary>
        public DeliveryDetailViewModel DeliveryDetailViewModel
        {
            get => _deliveryDetailViewModel;
            set => _ = SetProperty(ref _deliveryDetailViewModel, value);
        }

        /// <summary>
        /// Коллекция деталей
        /// </summary>
        public ObservableCollection<PartsInContainer> Parts
        {
            get => _parts;
            set => _ = SetProperty(ref _parts, value);
        }

        /// <summary>
        /// Коллекция контейнеров
        /// </summary>
        public ObservableCollection<ContainersInLot> Containers
        {
            get => _containers;
            set => _ = SetProperty(ref _containers, value);
        }

        /// <summary>
        /// Выбранный контейнер в списке
        /// </summary>
        public ContainersInLot SelectedContainer
        {
            get => _selectedContainer;
            set
            {
                _ = SetProperty(ref _selectedContainer, value);

                if (SelectedContainer is not null)
                {
                    TitleParts = Resources.TitlePartsForContainer;

                    TitleParts += SelectedContainer.ContainerNumber;

                    LoadPartsForContainerAsync(SelectedContainer.Id);
                }
                else
                {
                    TitleParts = Resources.TitlePartsForContainer;
                }
            }
        }

        #region Filters

        public string FilterCaseNumber
        {
            get => _filterCaseNumber;
            set
            {
                if (_filterCaseNumber != value)
                {
                    _ = SetProperty(ref _filterCaseNumber, value);

                    UpdatePartsFilters();
                }
            }
        }

        public string FilterInvoiceNumber
        {
            get => _filterInvoiceNumber;
            set
            {
                if (_filterInvoiceNumber != value)
                {
                    _ = SetProperty(ref _filterInvoiceNumber, value);

                    UpdatePartsFilters();
                }
            }
        }

        public string FilterPartNameEng
        {
            get => _filterPartNameEng;
            set
            {
                if (_filterPartNameEng != value)
                {
                    _ = SetProperty(ref _filterPartNameEng, value);

                    UpdatePartsFilters();
                }
            }
        }

        public string FilterQuantity
        {
            get => _filterQuantity;
            set
            {
                if (_filterQuantity != value)
                {
                    _ = SetProperty(ref _filterQuantity, value);

                    UpdatePartsFilters();
                }
            }
        }

        public string FilterPartNumber
        {
            get => _filterPartNumber;
            set
            {
                if (_filterPartNumber != value)
                {
                    _ = SetProperty(ref _filterPartNumber, value);

                    UpdatePartsFilters();
                }
            }
        }

        public string FilterSupplierPackingType
        {
            get => _filterSupplierPackingType;
            set
            {
                if (_filterSupplierPackingType != value)
                {
                    _ = SetProperty(ref _filterSupplierPackingType, value);

                    UpdatePartsFilters();
                }
            }
        }

        public string FilterNetWeight
        {
            get => _filterNetWeight;
            set
            {
                if (_filterNetWeight != value)
                {
                    _ = SetProperty(ref _filterNetWeight, value);

                    UpdatePartsFilters();
                }
            }
        }

        public string FilterGrossWeight
        {
            get => _filterGrossWeight;
            set
            {
                if (_filterGrossWeight != value)
                {
                    _ = SetProperty(ref _filterGrossWeight, value);

                    UpdatePartsFilters();
                }
            }
        }

        public string FilterContainerType
        {
            get => _filterContainerType;
            set
            {
                if (_filterContainerType != value)
                {
                    _ = SetProperty(ref _filterContainerType, value);

                    UpdateContainersFilters();
                }
            }
        }

        public string FilterContainerNumber
        {
            get => _filterContainerNumber;
            set
            {
                if (_filterContainerNumber != value)
                {
                    _ = SetProperty(ref _filterContainerNumber, value);

                    UpdateContainersFilters();
                }
            }
        }

        public string FilterSealNumber
        {
            get => _filterSealNumber;
            set
            {
                if (_filterSealNumber != value)
                {
                    _ = SetProperty(ref _filterSealNumber, value);

                    UpdateContainersFilters();
                }
            }
        }

        public string FilterIMO
        {
            get => _filterIMO;
            set
            {
                if (_filterIMO != value)
                {
                    _ = SetProperty(ref _filterIMO, value);

                    UpdateContainersFilters();
                }
            }
        }

        public string FilterContainerComment
        {
            get => _filterContainerComment;
            set
            {
                if (_filterContainerComment != value)
                {
                    _ = SetProperty(ref _filterContainerComment, value);

                    UpdateContainersFilters();
                }
            }
        }

        #endregion 

        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo(object parameter)
        {
            if (parameter is int lotId)
            {
                _logger.LogInformation($"Start loading lot with uniq id '{lotId}'.");

                Lot lot = await GetLotByIdAsync(lotId);

                _logger.LogInformation($"Loading lot with uniq id '{lotId}' completed.");

                LotLoaded?.Invoke(this, lot);
            }
        }

        /// <summary>
        /// Обработчик события загрузки контейнеров
        /// </summary>
        private void OnContainersLoaded(object sender, List<ContainersInLot> containers)
        {
            if (Containers.Count != 0)
            {
                Containers.Clear();
            }

            foreach (ContainersInLot container in containers)
            {
                if (container is not null)
                {
                    Containers.Add(container);
                }
            }

            ContainersView = CollectionViewSource.GetDefaultView(Containers);

            UpdateContainersFilters();
        }

        /// <summary>
        /// Обработчик события загрузки лотов
        /// </summary>
        private async void OnLotLoadedAsync(object sender, Lot lot)
        {
            if (lot is not null)
            {
                if (DeliveryDetailViewModel is null)
                {
                    DeliveryDetailViewModel = new DeliveryDetailViewModel(new(), _staticDataService, _deliveryService, _logger, _progressController)
                    {
                        Carrier = lot.Carrier,
                        LotArrivalLocation = lot.LotArrivalLocation,
                        LotAta = lot.LotAta,
                        LotAtd = lot.LotAtd,
                        LotComment = lot.LotComment,
                        LotCustomsLocation = lot.LotCustomsLocation,
                        LotDepartureLocation = lot.LotDepartureLocation,
                        LotEta = lot.LotEta,
                        LotEtd = lot.LotEtd,
                        LotNumber = lot.LotNumber,
                        LotPurchaseOrder = lot.LotPurchaseOrder,
                        LotTransport = lot.LotTransport,
                        LotTransportDocument = lot.LotTransportDocument,
                        Shipper = lot.Shipper,
                        TermsOfDelivery = lot.DeliveryTerms,
                        TypeOfTransport = lot.LotTransportType,
                        CloseDate = lot.CloseDate
                    };
                }

                _logger.LogInformation($"Start loading containers for the lot with uniq id '{lot.Id}'.");

                ContainersLoaded?.Invoke(this, await GetContainersAsync(lot.Id));

                _logger.LogInformation($"Loading containers for the lot with uniq id '{lot.Id}' completed.");
            }
        }

        /// <summary>
        /// Згружает детали для выбранного контейнера
        /// </summary>    
        private async void LoadPartsForContainerAsync(int containerId)
        {
            try
            {
                await CreateController(Resources.BllLoadPartsForContainer);

                if (Parts.Count != 0)
                {
                    Parts.Clear();
                }

                _logger.LogInformation($"Start loading parts for the container with uniq id '{containerId}'.");

                foreach (PartsInContainer part in await _deliveryService.GetPartsForContainerAsync(containerId))
                {
                    if (part is not null)
                    {
                        Parts.Add(part);
                    }
                }

                _logger.LogInformation($"Loading parts for the container with uniq id '{containerId}' completed.");

                PartsView = CollectionViewSource.GetDefaultView(Parts);

                UpdatePartsFilters();
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
        /// Обновляет фильтры коллекции деталей
        /// </summary>
        private void UpdatePartsFilters()
        {
            _logger.LogInformation($"Start updating filters to represent details.");

            _partsFilters = new List<ICustomFilter>();

            if (!string.IsNullOrEmpty(FilterCaseNumber))
            {
                _partsFilters.Add(new CaseNumberFilter(FilterCaseNumber));
            }

            if (!string.IsNullOrEmpty(FilterInvoiceNumber))
            {
                _partsFilters.Add(new InvoiceNumberFilter(FilterInvoiceNumber));
            }

            if (!string.IsNullOrEmpty(FilterPartNameEng))
            {
                _partsFilters.Add(new PartNameEngFilter(FilterPartNameEng));
            }

            if (!string.IsNullOrEmpty(FilterQuantity))
            {
                _partsFilters.Add(new QuantityFilter(FilterQuantity));
            }

            if (!string.IsNullOrEmpty(FilterPartNumber))
            {
                _partsFilters.Add(new PartNumberFilter(FilterPartNumber));
            }

            if (!string.IsNullOrEmpty(FilterSupplierPackingType))
            {
                _partsFilters.Add(new SupplierPackingTypeFilter(FilterSupplierPackingType));
            }

            if (!string.IsNullOrEmpty(FilterNetWeight))
            {
                _partsFilters.Add(new NetWeightFilter(FilterNetWeight));
            }

            if (!string.IsNullOrEmpty(FilterGrossWeight))
            {
                _partsFilters.Add(new GrossWeightFilter(FilterGrossWeight));
            }

            _logger.LogInformation($"Updating filters to represent details completed.");

            ApplyPartsFilter();
        }

        /// <summary>
        /// Обновляет фильтры коллекции контейнеров
        /// </summary>
        private void UpdateContainersFilters()
        {
            _logger.LogInformation($"Start updating filters to represent containers.");

            _containersFilters = new List<ICustomFilter>();

            if (!string.IsNullOrEmpty(FilterContainerNumber))
            {
                _containersFilters.Add(new ContainerNumberFilter(FilterContainerNumber));
            }

            if (!string.IsNullOrEmpty(FilterContainerType))
            {
                _containersFilters.Add(new ContainerTypeFilter(FilterContainerType));
            }

            if (!string.IsNullOrEmpty(FilterSealNumber))
            {
                _containersFilters.Add(new SealNumberFilter(FilterSealNumber));
            }

            if (!string.IsNullOrEmpty(FilterIMO))
            {
                _containersFilters.Add(new IMOFilter(FilterIMO));
            }

            if (!string.IsNullOrEmpty(FilterContainerComment))
            {
                _containersFilters.Add(new ContainerCommentFilter(FilterContainerComment));
            }

            _logger.LogInformation($"Updating filters to represent containers completed.");

            ApplyContainersFilter();
        }

        /// <summary>
        /// Применяет фильтры к коллекции деталей
        /// </summary>
        private void ApplyPartsFilter()
        {
            _logger.LogInformation($"Start apply filters to represent details.");

            if (PartsView is not null)
            {
                PartsView.Filter = item =>
                {
                    return _partsFilters.All(filter => filter.PassesFilter(item));
                };

                PartsView.Refresh();
            }

            _logger.LogInformation($"Apply filters to represent details completed.");
        }

        /// <summary>
        /// Применяет фильтры к коллекции контейнеров
        /// </summary>
        private void ApplyContainersFilter()
        {
            _logger.LogInformation($"Start apply filters to represent containers.");

            if (ContainersView is not null)
            {
                ContainersView.Filter = item =>
                {
                    return _containersFilters.All(filter => filter.PassesFilter(item));
                };

                ContainersView.Refresh();
            }

            _logger.LogInformation($"Apply filters to represent containers completed.");
        }

        /// <summary>
        /// Получает список контейнеров по уникальному идентификатору лота
        /// </summary>
        /// <param name="lotId">уникальный идентификатор лота</param>
        /// <returns>Затача представляющая асинхронную операцию, возвращающую список контейнеров по уникальному идентификатору лота</returns>
        private async Task<List<ContainersInLot>> GetContainersAsync(int lotId)
        {
            try
            {
                await CreateController(Resources.BllDownloadContainers);

                return await _deliveryService.GetAllContainersByLotIdAsync(lotId);
            }
            catch (Exception ex)
            {
                await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);

                return null;
            }
            finally
            {
                await ControllerPostProcess();
            }
        }

        /// <summary>
        /// Получает лот по уникальному идентификатору
        /// </summary>
        /// <param name="lotId">уникальный идентификатор лота</param>
        /// <returns>Задача представляющая асинхронную операцию возвращающую лот по уникальному идентификатору</returns>
        private async Task<Lot> GetLotByIdAsync(int lotId)
        {
            try
            {
                await CreateController(Resources.BllDownloadLot);

                return await _deliveryService.GetLotByIdAsync(lotId);
            }
            catch (Exception ex)
            {
                await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);

                return null;
            }
            finally
            {
                await ControllerPostProcess();
            }
        }

        /// <summary>
        /// Редактирует детальную информацию
        /// </summary>
        private async Task EditDetailsAsync()
        {
            try
            {
                await CreateController(Resources.EditDeliveryPageTitle);

                throw new Exception("Этот функционал находится в разработке.");

            }
            catch (Exception ex)
            {
                await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);

                return;
            }
            finally
            {
                await ControllerPostProcess();

                CloseDetailsFlyout();
            }
        }

        /// <summary>
        /// Открывает панель с детальной информацией
        /// </summary>
        private void OpenDetailsFlyout()
        {
            DetailsOpen = true;
        }

        /// <summary>
        /// Закрывает панель с детальной информацией
        /// </summary>
        private void CloseDetailsFlyout()
        {
            DetailsOpen = !DetailsOpen;
        }

        /// <summary>
        /// Событие, уведомляющее о загрузке лота.
        /// </summary>
        public event EventHandler<Lot> LotLoaded;

        /// <summary>
        /// Событие, уведомляющее о загрузке контейнеров.
        /// </summary>
        public event EventHandler<List<ContainersInLot>> ContainersLoaded;
    }
}
