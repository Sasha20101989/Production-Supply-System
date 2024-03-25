using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

using BLL.Contracts;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MahApps.Metro.Controls.Dialogs;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

using production_supply_system.EntityFramework.DAL.Enums;
using production_supply_system.EntityFramework.DAL.LotContext.Models;

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

        private string _transportName;

        private string _newTransportName;

        private Shipper _shipper;

        private Transport _lotTransport;

        [ObservableProperty]
        private bool _isTransportDropDownOpen;

        [ObservableProperty]
        private bool _isAddTransportVisible = false;

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

        [ObservableProperty]
        private string _lotTransportDocument;

        [ObservableProperty]
        private DateTime? _closeDate;

        [ObservableProperty]
        private DateTime? _lotEtd;

        [ObservableProperty]
        private DateTime? _lotAtd;

        [ObservableProperty]
        private DateTime? _lotEta;

        [ObservableProperty]
        private DateTime? _lotAta;

        [ObservableProperty]
        private TypesOfTransport _lotTransportType;

        [ObservableProperty]
        private TermsOfDelivery _deliveryTerms;

        [ObservableProperty]
        private PurchaseOrder _lotPurchaseOrder;

        [ObservableProperty]
        private Carrier _carrier;

        [ObservableProperty]
        private string _lotNumber;

        [ObservableProperty]
        private string _lotComment;

        [ObservableProperty]
        private Location _lotDepartureLocation;

        [ObservableProperty]
        private Location _lotCustomsLocation;

        [ObservableProperty]
        private Location _lotArrivalLocation;

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

            _ = Init();
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
            get => _lotTransport;
            set
            {
                IsAddTransportVisible = IsExistsTransport(value);

                if (!IsAddTransportVisible)
                {
                    if (value is not null)
                    {
                        _ = SetProperty(ref _lotTransport, value);
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
            get => _shipper;
            set
            {
                _ = SetProperty(ref _shipper, value);

                if (value is not null)
                {
                    PurchaseOrdersForShipper = Shipper.PurchaseOrders.Count == 0 ?
                        _deliveryService.GetPurchaseOrdersForShipper(Shipper) :
                        [.. Shipper.PurchaseOrders];
                }
            }
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

                LotTransport = newTransport;

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

                Shippers = await _staticDataService.GetAllShippersAsync();

                Carriers = await _staticDataService.GetAllCarriersAsync();

                TermsOfDeliveryItems = await _staticDataService.GetAllTermsOfDeliveryAsync();

                TypesOfTransport = await _staticDataService.GetAllTransportTypesAsync();

                CustomsLocations = await _staticDataService.GetLocationsByTypeAsync(LocationType.CustomsTerminal);

                ArrivalLocations = await _staticDataService.GetLocationsByTypeAsync(LocationType.ArrivalTerminal);

                DepartureLocations = await _staticDataService.GetLocationsByTypeAsync(LocationType.DepartureTerminal);

                _logger.LogInformation($"{Resources.LogLoadStaticData} {Resources.Completed}");

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
