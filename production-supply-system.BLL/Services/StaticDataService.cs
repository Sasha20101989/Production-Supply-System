using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BLL.Contracts;

using DAL.Data.Repositories.Contracts;
using DAL.Enums;
using DAL.Extensions;
using DAL.Models;
using DAL.Models.Partscontrol;
using DAL.Parameters.Inbound;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Serilog;

namespace BLL.Services
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IRepository<Shipper> _shipperRepository;
        private readonly IRepository<Carrier> _carrierRepository;
        private readonly IRepository<TermsOfDelivery> _termsOfDeliveryRepository;
        private readonly IRepository<TypesOfTransport> _transportTypeRepository;
        private readonly IRepository<TypesOfLocation> _locationTypeRepository;
        private readonly IRepository<TypesOfOrder> _orderTypesRepository;
        private readonly IRepository<TypesOfPacking> _packingTypesRepository;
        private readonly IRepository<TypesOfPart> _partTypeRepository;
        private readonly IRepository<TypesOfContainer> _containerTypeRepository;
        private readonly IRepository<Transport> _transportRepository;
        private readonly IRepository<Location> _locationRepository;
        private readonly ILogger<StaticDataService> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="StaticDataService"/>.
        /// </summary>
        /// <param name="shipperRepository">Репозиторий для доступа к информации о отправителях.</param>
        /// <param name="carrierRepository">Репозиторий для доступа к информации о перевозчиках.</param>
        /// <param name="termsOfDeliveryRepository">Репозиторий для доступа к информации о условиях поставки.</param>
        /// <param name="transportTypeRepository">Репозиторий для доступа к информации о типах транспорта.</param>
        /// <param name="locationRepository">Репозиторий для доступа к информации о локациях.</param>
        /// <param name="locationTypeRepository">Репозиторий для доступа к информации о типах локаций.</param>
        /// <param name="transportRepository">Репозиторий для доступа к информации о транспорте.</param>
        /// <param name="containerTypeRepository">Репозиторий для доступа к информации о типах контейнеров.</param>
        /// <param name="logger">Регистратор для отслеживания информации и ошибок.</param>
        public StaticDataService(
            IRepository<Shipper> shipperRepository,
            IRepository<Carrier> carrierRepository,
            IRepository<TermsOfDelivery> termsOfDeliveryRepository,
            IRepository<TypesOfTransport> transportTypeRepository,
            IRepository<Location> locationRepository,
            IRepository<TypesOfLocation> locationTypeRepository,
            IRepository<Transport> transportRepository,
            IRepository<TypesOfContainer> containerTypeRepository,
            IRepository<TypesOfPart> partTypeRepository,
            IRepository<TypesOfOrder> orderTypesRepository,
            IRepository<TypesOfPacking> packingTypesRepository,
            ILogger<StaticDataService> logger)
        {
            _shipperRepository = shipperRepository;
            _carrierRepository = carrierRepository;
            _termsOfDeliveryRepository = termsOfDeliveryRepository;
            _transportTypeRepository = transportTypeRepository;
            _locationRepository = locationRepository;
            _partTypeRepository = partTypeRepository;
            _locationTypeRepository = locationTypeRepository;
            _transportRepository = transportRepository;
            _containerTypeRepository = containerTypeRepository;
            _packingTypesRepository = packingTypesRepository;
            _orderTypesRepository = orderTypesRepository;
            _logger = logger;
        }

        #region Get by id

        /// <inheritdoc />
        public async Task<TypesOfContainer> GetContainerTypeById(int containerTypeId)
        {
            try
            {
                return await _containerTypeRepository.GetByIdAsync(containerTypeId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get container type by id {containerTypeId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения типа контейнера по уникальному идентификатору '{containerTypeId}'.");
            }
        }

        /// <inheritdoc />
        public async Task<TypesOfPart> GetPartTypeByIdAsync(int partTypeId)
        {
            try
            {
                return await _partTypeRepository.GetByIdAsync(partTypeId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get part type by id {partTypeId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения типа детали по уникальному идентификатору '{partTypeId}'.");
            }
        }

        /// <inheritdoc />
        public async Task<TypesOfPacking> GetPackingTypeByIdAsync(int packingTypeId)
        {
            try
            {
                return await _packingTypesRepository.GetByIdAsync(packingTypeId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get packing type by id {packingTypeId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения типа упаковки по уникальному идентификатору '{packingTypeId}'.");
            }
        }

        /// <inheritdoc />
        public async Task<TypesOfLocation> GetLocationTypeByIdAsync(int locationTypeId)
        {
            try
            {
                return await _locationTypeRepository.GetByIdAsync(locationTypeId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get location by location type id {locationTypeId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения типа локации по уникальному идентификатору '{locationTypeId}'");
            }
        }

        /// <inheritdoc />
        public async Task<Carrier> GetCarrierByIdAsync(int carrierId)
        {
            try
            {
                return await _carrierRepository.GetByIdAsync(carrierId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get carrier by id {carrierId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения перевозчика по уникальному идентификатору '{carrierId}'");
            }
        }

        /// <inheritdoc />
        public async Task<TermsOfDelivery> GetDeliveryTermByIdAsync(int deliveryTermId)
        {
            try
            {
                return await _termsOfDeliveryRepository.GetByIdAsync(deliveryTermId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get delivery term by id {deliveryTermId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения условия поставки по уникальному идентификатору '{deliveryTermId}'");
            }
        }

        /// <inheritdoc />
        public async Task<Location> GetLocationByIdAsync(int locationId)
        {
            try
            {
                Location location = await _locationRepository.GetByIdAsync(locationId);

                if (location is not null)
                {
                    location.LocationType = await GetLocationTypeByIdAsync(location.LocationTypeId);
                }

                return location;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get location by id {locationId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения локации по уникальному идентификатору '{locationId}'");
            }
        }

        /// <inheritdoc />
        public async Task<Transport> GetTransportByIdAsync(int transportId)
        {
            try
            {
                return await _transportRepository.GetByIdAsync(transportId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get transport by id {transportId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения транспорта по уникальному идентификатору '{transportId}'");
            }
        }

        /// <inheritdoc />
        public async Task<TypesOfTransport> GetTransportTypeByIdAsync(int transportTypeId)
        {
            try
            {
                return await _transportTypeRepository.GetByIdAsync(transportTypeId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get transport type by id {transportTypeId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения типа транспорта по уникальному идентификатору '{transportTypeId}'");
            }
        }

        /// <inheritdoc />
        public async Task<Shipper> GetShipperByIdAsync(int shipperId)
        {
            try
            {
                Shipper shipper = await _shipperRepository.GetByIdAsync(shipperId);

                if (shipper is not null)
                {
                    if (shipper.ShipperDefaultDeliveryLocationId is not null)
                    {
                        shipper.ShipperDefaultDeliveryLocation = await GetLocationByIdAsync((int)shipper.ShipperDefaultDeliveryLocationId);
                    }
                }

                return shipper;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get shipper by id {shipperId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения отправителя по уникальному идентификатору '{shipperId}'.");
            }
        }

        /// <inheritdoc />
        public async Task<TypesOfOrder> GetPurchaseOrderTypeById(int orderTypeId)
        {
            try
            {
                return await _orderTypesRepository.GetByIdAsync(orderTypeId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get order type by id {orderTypeId}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения типа заказа по уникальному идентификатору '{orderTypeId}'");
            }
        }

        #endregion Get by id

        #region Get All

        public async Task<IEnumerable<TypesOfPart>> GetAllPartTypesAsync()
        {
            try
            {
                return await _partTypeRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get all part types list: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка типов деталей.");
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TypesOfPacking>> GetAllPackingTypesAsync()
        {
            try
            {
                return await _packingTypesRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get all packing types list: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка типов упаковок.");
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Shipper>> GetAllShippersAsync()
        {
            try
            {
                return await _shipperRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list shippers: {JsonConvert.SerializeObject(ex)}");

                throw new Exception("Ошибка получения грузоотправителей.");
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Carrier>> GetAllCarriersAsync()
        {
            try
            {
                return await _carrierRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list carriers: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка перевозчиков.");
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TermsOfDelivery>> GetAllTermsOfDeliveryAsync()
        {
            try
            {
                return await _termsOfDeliveryRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list terms of delivery: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка условий поставки.");
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TypesOfTransport>> GetAllTransportTypesAsync()
        {
            try
            {
                return await _transportTypeRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list transport types: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка типов транспорта.");
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            try
            {
                IEnumerable<Location> locations = await _locationRepository.GetAllAsync();

                foreach (Location location in locations)
                {
                    TypesOfLocation locationType = await GetLocationTypeByIdAsync(location.LocationTypeId);

                    location.LocationType = locationType;
                }

                return locations;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list locations: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения локаций.");
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Transport>> GetAllTransportsAsync()
        {
            try
            {
                return await _transportRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list transports: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения списка транспорта.");
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TypesOfContainer>> GetAllContainerTypes()
        {
            try
            {
                return await _containerTypeRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list container types: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка получения типов контейнера.");
            }
        }

        #endregion Get All

        /// <inheritdoc />
        public async Task<TypesOfPacking> GetExistingPackingTypeByTypeAsync(string packingType)
        {
            _logger.LogInformation($"The beginning of receiving a package type by type name '{packingType}'");

            TypesOfPacking result = (await GetAllPackingTypesAsync()).FirstOrDefault(c => c.SupplierPackingType == packingType);

            _logger.LogInformation($"Receiving a package type by type name '{packingType}' completed, result: '{JsonConvert.SerializeObject(result)}'");

            return result;
        }

        /// <inheritdoc />
        public async Task<Transport> AddTransportAsync(Transport newTransport)
        {
            CreateTransportParameters parameters = new(newTransport);

            try
            {
                newTransport = await _transportRepository.CreateAsync(newTransport, StoredProcedureInbound.AddNewTransport, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new transport: {JsonConvert.SerializeObject(newTransport)}: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка добавления нового транспорта.");
            }

            try
            {
                _transportRepository.RefreshData();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error refresh list transports: {JsonConvert.SerializeObject(ex)}");

                throw new Exception($"Ошибка обновления списка транспорта.");
            }

            return newTransport;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Location>> GetLocationsByTypeAsync(LocationType locationType)
        {
            IEnumerable<Location> locations = await GetAllLocationsAsync();

            string locationTypeName = EnumExtensions.GetDescription(locationType);

            _logger.LogInformation($"Filter locations by location type {locationTypeName}");

            return locations.Where(location => location.LocationType.LocationType == locationTypeName);
        }

        /// <inheritdoc />
        public async Task<TypesOfContainer> GetContainerTypeByName(string containerTypeName)
        {
            _logger.LogInformation($"The beginning of receiving a container by the name of the container type: '{containerTypeName}'");

            TypesOfContainer result = (await GetAllContainerTypes())
                      .FirstOrDefault(ct => ct.ContainerType == containerTypeName);

            _logger.LogInformation($"Receiving a container by the name of the container type: '{containerTypeName}' completed, result: '{JsonConvert.SerializeObject(result)}'");

            return result;
        }

        /// <inheritdoc />
        public async Task<Location> GetFinalLocationAsync()
        {
            _logger.LogInformation($"The beginning of receiving a final location");

            Location result = (await GetAllLocationsAsync()).
                FirstOrDefault(l => l.LocationType.LocationType == EnumExtensions.GetDescription(LocationType.FinalLocation));
            _logger.LogInformation($"Receiving a final location completed, result: '{JsonConvert.SerializeObject(result)}'");

            return result;
        }

        /// <inheritdoc />
        public async Task<TypesOfPart> GetPartTypeByNameAsync(PartTypes partType)
        {
            _logger.LogInformation($"The beginning of receiving a part type by the name: '{partType}'");

            TypesOfPart result = (await GetAllPartTypesAsync()).FirstOrDefault(pt => pt.PartType == partType);

            _logger.LogInformation($"The beginning of receiving a part type by the name: '{partType}' completed, result: '{JsonConvert.SerializeObject(result)}'");

            return result;
        }
    }
}
