using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

using BLL.Contracts;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using NavigationManager.Frame.Extension.WPF;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

using UI_Interface.Contracts;
using UI_Interface.Extensions;
using UI_Interface.Filters;
using UI_Interface.Properties;

namespace UI_Interface.ViewModels.ViewModelsForPages
{
    /// <summary>
    /// ViewModel, представляющая информацию о лоте для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ControlledViewModel для уведомлений об изменении свойств и выдачи уведомлений.
    /// </summary>
    public partial class EditDeliveryViewModel(
        IStaticDataService staticDataService,
        IDeliveryService deliveryService,
        ILogger<EditDeliveryViewModel> logger) : ControlledViewModel(logger), INavigationAware
    {
        [ObservableProperty]
        private DeliveryDetailViewModel _deliveryDetailViewModel;

        [ObservableProperty]
        private ObservableCollection<PartsInContainer> _parts = [];

        [ObservableProperty]
        private ObservableCollection<ContainersInLot> _containers = [];

        [ObservableProperty]
        private string _titleParts = Resources.TitlePartsForContainer;

        [ObservableProperty]
        private decimal _totalQuantityNetWeight;

        [ObservableProperty]
        private decimal _totalQuantityGrossWeight;

        [ObservableProperty]
        private decimal _totalQuantityParts;

        [ObservableProperty]
        private ICollectionView _partsView;

        [ObservableProperty]
        private ICollectionView _containersView;

        [ObservableProperty]
        private bool _detailsOpen = false;

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

        private List<ICustomFilter> _partsFilters;
        private List<ICustomFilter> _containersFilters;

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

                    CreatePartsForContainer();
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
                Lot lot = await GetLotByIdAsync(lotId);

                if (lot is not null)
                {
                    CreateContainers(lot);

                    CreateDetails(lot);
                }
            }
        }

        /// <summary>
        /// Редактирует детальную информацию
        /// </summary>
        [RelayCommand]
        private async Task EditDetailsAsync()
        {
            try
            {
                await CreateController(Resources.EditDeliveryPageTitle);

                throw new Exception("Этот функционал находится в разработке");

            }
            catch (Exception ex)
            {
                await WaitForMessageUnlock(Resources.ShellError, ex.Message, Brushes.IndianRed);

                return;
            }
            finally
            {
                await ControllerPostProcess();

                CloseDetails();
            }
        }

        /// <summary>
        /// Открывает панель с детальной информацией
        /// </summary>
        [RelayCommand]
        private void OpenDetails()
        {
            DetailsOpen = true;
        }

        /// <summary>
        /// Закрывает панель с детальной информацией
        /// </summary>
        [RelayCommand]
        private void CloseDetails()
        {
            DetailsOpen = !DetailsOpen;
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
                logger.LogInformation(string.Format(Resources.LogLotGetById, lotId));

                await CreateController(Resources.BllDownloadLot);

                Lot lot = await deliveryService.GetLotByIdAsync(lotId);

                logger.LogInformation($"{string.Format(Resources.LogLotGetById, lotId)} {Resources.Completed}");

                return lot;
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
        /// Устанавливает детали для выбранного контейнера
        /// </summary>    
        private void CreatePartsForContainer()
        {
            if (Parts.Count != 0)
            {
                Parts.Clear();
            }

            Parts.AddRange(SelectedContainer.PartsInContainers);

            TotalQuantityGrossWeight = Parts.Sum(p => p.Case.GrossWeight);

            TotalQuantityNetWeight = Parts.Sum(p => p.Case.NetWeight);

            TotalQuantityParts = Parts.Sum(p => p.Quantity);

            PartsView = CollectionViewSource.GetDefaultView(Parts);

            UpdatePartsFilters();
        }

        /// <summary>
        /// Устанавливает контейнеры для выбранного лота
        /// </summary> 
        private void CreateContainers(Lot lot)
        {
            if (Containers.Count != 0)
            {
                Containers.Clear();
            }

            Containers.AddRange(lot.ContainersInLots);

            ContainersView = CollectionViewSource.GetDefaultView(Containers);

            UpdateContainersFilters();
        }

        /// <summary>
        /// Устанавливает детальную информацию для выбранного лота
        /// </summary> 
        private void CreateDetails(Lot lot)
        {
            DeliveryDetailViewModel ??= new DeliveryDetailViewModel([], staticDataService, deliveryService, logger, _progressController)
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
                DeliveryTerms = lot.DeliveryTerms,
                LotTransportType = lot.LotTransportType,
                CloseDate = lot.CloseDate
            };
        }

        /// <summary>
        /// Обновляет фильтры коллекции деталей
        /// </summary>
        private void UpdatePartsFilters()
        {
            logger.LogInformation(Resources.LogUpdatingFiltersForDetails);

            _partsFilters = [];

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

            logger.LogInformation($"{Resources.LogUpdatingFiltersForDetails} {Resources.Completed}");

            ApplyPartsFilter();
        }

        /// <summary>
        /// Обновляет фильтры коллекции контейнеров
        /// </summary>
        private void UpdateContainersFilters()
        {
            logger.LogInformation(Resources.LogUpdatingFiltersForContainers);

            _containersFilters = [];

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

            logger.LogInformation($"{Resources.LogUpdatingFiltersForContainers} {Resources.Completed}");

            ApplyContainersFilter();
        }

        /// <summary>
        /// Применяет фильтры к коллекции деталей
        /// </summary>
        private void ApplyPartsFilter()
        {
            logger.LogInformation(Resources.LogUpplyFiltersForDetails);

            if (PartsView is not null)
            {
                PartsView.Filter = item =>
                {
                    return _partsFilters.All(filter => filter.PassesFilter(item));
                };

                PartsView.Refresh();
            }

            logger.LogInformation($"{Resources.LogUpplyFiltersForDetails} {Resources.Completed}");
        }

        /// <summary>
        /// Применяет фильтры к коллекции контейнеров
        /// </summary>
        private void ApplyContainersFilter()
        {
            logger.LogInformation(Resources.LogUpplyFiltersForContainers);

            if (ContainersView is not null)
            {
                ContainersView.Filter = item =>
                {
                    return _containersFilters.All(filter => filter.PassesFilter(item));
                };

                ContainersView.Refresh();
            }

            logger.LogInformation($"{Resources.LogUpplyFiltersForContainers} {Resources.Completed}");
        }
    }
}
