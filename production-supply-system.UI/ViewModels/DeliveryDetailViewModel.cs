using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using BLL.Contracts;
using DAL.Enums;
using DAL.Models;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;

using Newtonsoft.Json;

using UI_Interface.Properties;
using UI_Interface.ViewModels.ViewModelsForPages;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// ViewModel, представляющая информацию о необходимых данных перед парсингом инвойса для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ObservableObject для уведомлений об изменении свойств.
    /// Реализует IDataErrorInfo для поддержки валидации данных.
    /// </summary>
    public class DeliveryDetailViewModel : ValidatedViewModel<DeliveryDetailViewModel, List<Type>>
    {
        private ProgressDialogController _progressController;

        private readonly ILogger _logger;

        private readonly IStaticDataService _staticDataService;

        private readonly IDeliveryService _deliveryService;

        private bool _isTransportDropDownOpen;

        private bool _isAddTransportVisible;

        private string _transportName;

        private string _newTransportName;

        private Lot _lot;

        private List<Location> _departureLocations;

        private List<Location> _customsLocations;

        private List<Location> _arrivalLocations;

        private List<Shipper> _shippers;

        private List<Carrier> _carriers;

        private List<TermsOfDelivery> _termsOfDeliveryItems;

        private List<TypesOfTransport> _typesOfTransport;

        private List<PurchaseOrder> _allPurchaseOrders;

        private List<PurchaseOrder> _purchaseOrdersForShipper;

        private ObservableCollection<Transport> _transports;


        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DeliveryDetailViewModel"/>.
        /// </summary>
        /// <param name="models">Валидируемые модели</param>
        /// <param name="staticDataService">Сервис взаимодействия со статическими данными</param>
        public DeliveryDetailViewModel(List<Type> models, IStaticDataService staticDataService, IDeliveryService deliveryService, ILogger logger, ProgressDialogController progressController) : base(models, logger)
        {
            _logger = logger;

            _staticDataService = staticDataService;

            _deliveryService = deliveryService;

            _progressController = progressController;

            _lot = new();

            _lot.LotTransport = new();

            AddNewTransportCommand = new AsyncRelayCommand(OnAddNewTransport);

            IsAddTransportVisible = false;

            _ = Init();
        }

        /// <summary>
        /// Команда добавления нового транспорта
        /// </summary>
        public AsyncRelayCommand AddNewTransportCommand { get; }

        /// <summary>
        /// Получает или задает транспорт.
        /// </summary>
        public ObservableCollection<Transport> Transports
        {
            get => _transports;
            set => _ = SetProperty(ref _transports, value);
        }

        /// <summary>
        /// Получает или задает видимость кнопки,
        /// если введённый пользователем транспорт не обнаружен 
        /// то кнопка добавления нового транспорта становится видимой
        /// </summary>
        public bool IsAddTransportVisible
        {
            get => _isAddTransportVisible;
            set => _ = SetProperty(ref _isAddTransportVisible, value);
        }

        /// <summary>
        /// Получает или задает состояние комбобокса(открыт/закрыт)
        /// </summary>
        public bool IsTransportDropDownOpen
        {
            get => _isTransportDropDownOpen;
            set => _ = SetProperty(ref _isTransportDropDownOpen, value);
        }

        /// <summary>
        /// Получает или задает номер документа.
        /// </summary>
        public string LotTransportDocument
        {
            get => Lot.LotTransportDocument;
            set => _ = SetProperty(Lot.LotTransportDocument, value, Lot, (model, name) => model.LotTransportDocument = name);
        }

        /// <summary>
        /// Получает или задает дату закрытия лота.
        /// </summary>
        public DateTime? CloseDate
        {
            get => Lot.CloseDate;
            set => _ = SetProperty(Lot.CloseDate, value, Lot, (model, date) => model.CloseDate = date);
        }

        /// <summary>
        /// Получает или задает Expected time Delivery.
        /// </summary>
        public DateTime? LotEtd
        {
            get => Lot.LotEtd;
            set => _ = SetProperty(Lot.LotEtd, value, Lot, (model, date) => model.LotEtd = date);
        }

        /// <summary>
        /// Получает или задает Actual time Delivery.
        /// </summary>
        public DateTime? LotAtd
        {
            get => Lot.LotAtd;
            set => _ = SetProperty(Lot.LotAtd, value, Lot, (model, date) => model.LotAtd = date);
        }

        /// <summary>
        /// Получает или задает Expected time Arrival.
        /// </summary>
        public DateTime? LotEta
        {
            get => Lot.LotEta;
            set => _ = SetProperty(Lot.LotEta, value, Lot, (model, date) => model.LotEta = date);
        }

        /// <summary>
        /// Получает или задает Actual time Arrival.
        /// </summary>
        public DateTime? LotAta
        {
            get => Lot.LotAta;
            set => _ = SetProperty(Lot.LotAta, value, Lot, (model, date) => model.LotAta = date);
        }

        /// <summary>
        /// Получает или задает тип транспорта.
        /// </summary>
        public TypesOfTransport TypeOfTransport
        {
            get => Lot.LotTransportType;
            set => _ = SetProperty(Lot.LotTransportType, value, Lot, (model, type) => model.LotTransportType = type);
        }

        /// <summary>
        /// Получает или задает условия поставки.
        /// </summary>
        public TermsOfDelivery TermsOfDelivery
        {
            get => Lot.DeliveryTerms;
            set => _ = SetProperty(Lot.DeliveryTerms, value, Lot, (model, terms) => model.DeliveryTerms = terms);
        }

        /// <summary>
        /// Получает или задает имя транспорта.
        /// </summary>
        public string TransportName
        {
            get => _transportName;
            set
            {
                IsAddTransportVisible = IsExistsTransport(value);

                _ = !IsAddTransportVisible ? SetProperty(ref _transportName, value) : SetProperty(ref _newTransportName, value);
            }
        }

        /// <summary>
        /// Получает или задаёт транспорт
        /// </summary>
        public Transport LotTransport
        {
            get => Lot.LotTransport;
            set
            {
                IsAddTransportVisible = IsExistsTransport(value);

                if (!IsAddTransportVisible)
                {
                    if (value is not null)
                    {
                        _ = SetProperty(Lot.LotTransport, value, Lot, (model, transport) => model.LotTransport = transport);
                        _ = SetProperty(ref _transportName, value.TransportName);
                    }
                }
            }
        }

        /// <summary>
        /// Получает или задает отправителя.
        /// </summary>
        public Shipper Shipper
        {
            get => Lot.Shipper;
            set
            {
                _ = SetProperty(Lot.Shipper, value, Lot, (model, shipper) => model.Shipper = shipper);

                LoadPurchaseOrdersForShipper();
            }
        }

        /// <summary>
        /// Получает или задает заказы.
        /// </summary>
        public List<PurchaseOrder> AllPurchaseOrders
        {
            get => _allPurchaseOrders;
            set => _ = SetProperty(ref _allPurchaseOrders, value);
        }

        /// <summary>
        /// Получает или задает заказы для отправителя.
        /// </summary>
        public List<PurchaseOrder> PurchaseOrdersForShipper
        {
            get => _purchaseOrdersForShipper;
            set => _ = SetProperty(ref _purchaseOrdersForShipper, value);
        }

        /// <summary>
        /// Получает или задает заказ.
        /// </summary>
        public PurchaseOrder LotPurchaseOrder
        {
            get => Lot.LotPurchaseOrder;
            set => _ = SetProperty(Lot.LotPurchaseOrder, value, Lot, (model, order) => model.LotPurchaseOrder = order);
        }

        /// <summary>
        /// Получает или задает перевозчика.
        /// </summary>
        public Carrier Carrier
        {
            get => Lot.Carrier;
            set => _ = SetProperty(Lot.Carrier, value, Lot, (model, carrier) => model.Carrier = carrier);
        }

        /// <summary>
        /// Получает или задает локации отправления лота.
        /// </summary>
        public List<Location> DepartureLocations
        {
            get => _departureLocations;
            set => _ = SetProperty(ref _departureLocations, value);
        }

        /// <summary>
        /// Получает или задает локации таможенного терминала.
        /// </summary>
        public List<Location> CustomsLocations
        {
            get => _customsLocations;
            set => _ = SetProperty(ref _customsLocations, value);
        }

        /// <summary>
        /// Получает или задает локации места прибытия.
        /// </summary>
        public List<Location> ArrivalLocations
        {
            get => _arrivalLocations;
            set => _ = SetProperty(ref _arrivalLocations, value);
        }

        /// <summary>
        /// Получает или задает типы транспорта.
        /// </summary>
        public List<TypesOfTransport> TypesOfTransport
        {
            get => _typesOfTransport;
            set => _ = SetProperty(ref _typesOfTransport, value);
        }

        /// <summary>
        /// Получает или задает условия поставки.
        /// </summary>
        public List<TermsOfDelivery> TermsOfDeliveryItems
        {
            get => _termsOfDeliveryItems;
            set => _ = SetProperty(ref _termsOfDeliveryItems, value);
        }

        /// <summary>
        /// Получает или задает перевозчиков.
        /// </summary>
        public List<Carrier> Carriers
        {
            get => _carriers;
            set => _ = SetProperty(ref _carriers, value);
        }

        /// <summary>
        /// Получает или задает отправителей.
        /// </summary>
        public List<Shipper> Shippers
        {
            get => _shippers;
            set => _ = SetProperty(ref _shippers, value);
        }

        /// <summary>
        /// Получает или задает лот.
        /// </summary>
        public Lot Lot
        {
            get => _lot;
            set => _ = SetProperty(ref _lot, value);
        }

        /// <summary>
        /// Получает или задает значение номера лота.
        /// </summary>
        public string LotNumber
        {
            get => Lot.LotNumber;
            set => _ = SetProperty(Lot.LotNumber, value, Lot, (model, number) => model.LotNumber = number);
        }

        /// <summary>
        /// Получает или задает комментарий для лота.
        /// </summary>
        public string LotComment
        {
            get => Lot.LotComment;
            set => _ = SetProperty(Lot.LotComment, value, Lot, (model, comment) => model.LotComment = comment);
        }

        /// <summary>
        /// Получает или задает место отправления лота.
        /// </summary>
        public Location LotDepartureLocation
        {
            get => Lot.LotDepartureLocation;
            set => _ = SetProperty(Lot.LotDepartureLocation, value, Lot, (model, number) => model.LotDepartureLocation = number);
        }

        /// <summary>
        /// Получает или задает местонахождение участка таможни.
        /// </summary>
        public Location LotCustomsLocation
        {
            get => Lot.LotCustomsLocation;
            set => _ = SetProperty(Lot.LotCustomsLocation, value, Lot, (model, number) => model.LotCustomsLocation = number);
        }

        /// <summary>
        /// Получает или задает место прибытия лота.
        /// </summary>
        public Location LotArrivalLocation
        {
            get => Lot.LotArrivalLocation;
            set => _ = SetProperty(Lot.LotArrivalLocation, value, Lot, (model, number) => model.LotArrivalLocation = number);
        }

        /// <summary>
        /// Получает заказы для отправителя
        /// </summary>
        private void LoadPurchaseOrdersForShipper()
        {
            if (Shipper is not null)
            {
                _logger.LogInformation($"The beginning of receiving orders for the shipper with uniq id '{Shipper.Id}'");

                PurchaseOrdersForShipper = AllPurchaseOrders.Where(shipper => shipper.Id == Shipper.Id).ToList();

                _logger.LogInformation($"Receiving orders for the shipper with uniq id '{Shipper.Id}' completed");
            }
        }

        /// <summary>
        /// Проверяет существует ли транспорт
        /// </summary>
        /// <param name="parameter">Имя транспорта</param>
        /// <returns>Возвращает true если транспорт уже существует, в противном случае false</returns>
        private bool IsExistsTransport(object transportName)
        {
            if (transportName is string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return false;
                }

                _logger.LogInformation($"Start filtering the list with transport with transport name '{transportName}'");

                List<Transport> filteredTransports = Transports
                        .Where(transport =>
                              transport.TransportName == name)
                        .ToList();

                _logger.LogInformation($"Filtering the list with transport with transport name '{transportName}' completed");

                return !filteredTransports.Any();
            }

            return false;
        }

        /// <summary>
        /// Добавляет новый транспорт
        /// </summary>
        /// <returns></returns>
        private async Task OnAddNewTransport()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_newTransportName))
                {
                    return;
                }

                Transport newTransport = new()
                {
                    TransportName = _newTransportName
                };

                _logger.LogInformation($"Start adding the new transport '{JsonConvert.SerializeObject(newTransport)}'");

                newTransport = await _staticDataService.AddTransportAsync(newTransport);

                _logger.LogInformation($"Adding the new transport '{JsonConvert.SerializeObject(newTransport)}' completed");

                Transports.Add(newTransport);

                Lot.LotTransport = newTransport;

                IsAddTransportVisible = false;
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
        /// Инициализация данных
        /// </summary>
        /// <returns></returns>
        private async Task Init()
        {
            try
            {
                _logger.LogInformation($"Start loading static data.");

                Shippers = (await _staticDataService.GetAllShippersAsync()).ToList();

                Carriers = (await _staticDataService.GetAllCarriersAsync()).ToList();

                TermsOfDeliveryItems = (await _staticDataService.GetAllTermsOfDeliveryAsync()).ToList();

                TypesOfTransport = (await _staticDataService.GetAllTransportTypesAsync()).ToList();

                CustomsLocations = (await _staticDataService.GetLocationsByTypeAsync(LocationType.CustomsTerminal)).ToList();

                ArrivalLocations = (await _staticDataService.GetLocationsByTypeAsync(LocationType.ArrivalTerminal)).ToList();

                DepartureLocations = (await _staticDataService.GetLocationsByTypeAsync(LocationType.DepartureTerminal)).ToList();

                _logger.LogInformation($"Loading static data completed.");

                _logger.LogInformation($"Start loading orders.");

                AllPurchaseOrders = (await _deliveryService.GetAllPurchaseOrdersAsync()).ToList();

                _logger.LogInformation($"Loading orders completed.");

                _logger.LogInformation($"Start loading all transport.");

                Transports = new(await _staticDataService.GetAllTransportsAsync());

                _logger.LogInformation($"Loading all transport completed.");
            }
            catch (Exception ex)
            {
                await WaitForMessageUnlock(Resources.BllLoadStaticData, ex.Message, Brushes.Red);
            }
            finally
            {
                await ControllerPostProcess();
            }
        }

        /// <summary>
        /// Ожидает разблокировки сообщения с указанным заголовком, текстом сообщения и цветом прогресс-бара.
        /// </summary>
        /// <param name="title">Заголовок сообщения.</param>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="color">Цвет прогресс-бара.</param>
        private async Task WaitForMessageUnlock(string title, string message, SolidColorBrush color)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _progressController.SetTitle(title);

                _progressController.SetMessage($"{message} Пожалуйста нажмите '{Resources.ShellClose}' для завершения.");

                _progressController.SetCancelable(true);

                _progressController.SetProgressBarForegroundBrush(color);

                TaskCompletionSource<bool> tcs = new();

                _progressController.Canceled += (sender, args) => tcs.SetResult(true);

                _ = await tcs.Task;
            }
        }

        /// <summary>
        /// Завершает процесс контроллера, если он открыт.
        /// </summary>
        private async Task ControllerPostProcess()
        {
            if (!(_progressController is null) && _progressController.IsOpen)
            {
                await _progressController.CloseAsync();
            }
        }
    }
}
