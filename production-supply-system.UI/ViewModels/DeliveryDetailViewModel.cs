using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using BLL.Contracts;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.Enums;
using DAL.Models;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UI_Interface.Properties;

namespace UI_Interface.ViewModels
{
    /// <summary>
    /// ViewModel, представляющая информацию о необходимых данных перед парсингом инвойса для взаимодействия с пользовательским интерфейсом.
    /// Наследует от ObservableObject для уведомлений об изменении свойств.
    /// Реализует IDataErrorInfo для поддержки валидации данных.
    /// </summary>
    public partial class DeliveryDetailViewModel : ValidatedViewModel<DeliveryDetailViewModel, List<Type>>
    {
        private readonly ProgressDialogController _progressController;

        private readonly ILogger _logger;

        private readonly IStaticDataService _staticDataService;

        private readonly IDeliveryService _deliveryService;

        [ObservableProperty]
        private bool _isTransportDropDownOpen;

        [ObservableProperty]
        private bool _isAddTransportVisible = false;

        private string _transportName;

        private string _newTransportName;

        [ObservableProperty]
        private Lot _lot;

        [ObservableProperty]
        private List<Location> _departureLocations;

        [ObservableProperty]
        private List<Location> _customsLocations;

        [ObservableProperty]
        private List<Location> _arrivalLocations;

        [ObservableProperty]
        private List<Shipper> _shippers;

        [ObservableProperty]
        private List<Carrier> _carriers;

        [ObservableProperty]
        private List<TermsOfDelivery> _termsOfDeliveryItems;

        [ObservableProperty]
        private List<TypesOfTransport> _typesOfTransport;

        [ObservableProperty]
        private List<PurchaseOrder> _allPurchaseOrders;

        [ObservableProperty]
        private List<PurchaseOrder> _purchaseOrdersForShipper;

        [ObservableProperty]
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

            _lot = new()
            {
                LotTransport = new()
            };

            _ = Init();
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
        /// Добавляет новый транспорт
        /// </summary>
        [RelayCommand]
        private async Task AddNewTransport()
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

                _logger.LogInformation(string.Format(Resources.LogTransportAdd, JsonConvert.SerializeObject(newTransport)));

                newTransport = await _staticDataService.AddTransportAsync(newTransport);

                _logger.LogInformation($"{string.Format(Resources.LogTransportAdd, JsonConvert.SerializeObject(newTransport))} {Resources.Completed}");

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
        /// Получает заказы для отправителя
        /// </summary>
        private void LoadPurchaseOrdersForShipper()
        {
            if (Shipper is not null)
            {
                _logger.LogInformation(string.Format(Resources.LogOrdersGetForShipper, Shipper.Id));

                PurchaseOrdersForShipper = AllPurchaseOrders.Where(shipper => shipper.Id == Shipper.Id).ToList();

                _logger.LogInformation($"{string.Format(Resources.LogOrdersGetForShipper, Shipper.Id)} {Resources.Completed}");
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

                _logger.LogInformation(string.Format(Resources.LogFilterTransport, JsonConvert.SerializeObject(transportName)));

                List<Transport> filteredTransports = Transports
                        .Where(transport =>
                              transport.TransportName == name)
                        .ToList();

                _logger.LogInformation($"{string.Format(Resources.LogFilterTransport, JsonConvert.SerializeObject(transportName))} {Resources.Completed}");

                return filteredTransports.Count == 0;
            }

            return false;
        }

        /// <summary>
        /// Инициализация данных
        /// </summary>
        /// <returns></returns>
        private async Task Init()
        {
            try
            {
                _logger.LogInformation(Resources.LogLoadStaticData);

                Shippers = (await _staticDataService.GetAllShippersAsync()).ToList();

                Carriers = (await _staticDataService.GetAllCarriersAsync()).ToList();

                TermsOfDeliveryItems = (await _staticDataService.GetAllTermsOfDeliveryAsync()).ToList();

                TypesOfTransport = (await _staticDataService.GetAllTransportTypesAsync()).ToList();

                CustomsLocations = (await _staticDataService.GetLocationsByTypeAsync(LocationType.CustomsTerminal)).ToList();

                ArrivalLocations = (await _staticDataService.GetLocationsByTypeAsync(LocationType.ArrivalTerminal)).ToList();

                DepartureLocations = (await _staticDataService.GetLocationsByTypeAsync(LocationType.DepartureTerminal)).ToList();

                _logger.LogInformation($"{Resources.LogLoadStaticData} {Resources.Completed}");

                AllPurchaseOrders = (await _deliveryService.GetAllPurchaseOrdersAsync()).ToList();

                Transports = new(await _staticDataService.GetAllTransportsAsync());
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

                _progressController.SetMessage($"{message}. {string.Format(Resources.PleasePress, Resources.ShellClose)}.");

                _logger.LogError(message);

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
            if (_progressController is not null && _progressController.IsOpen)
            {
                await _progressController.CloseAsync();
            }
        }
    }
}
