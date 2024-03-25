using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BLL.Contracts;
using BLL.Properties;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using production_supply_system.EntityFramework.DAL.Context;
using production_supply_system.EntityFramework.DAL.Enums;
using production_supply_system.EntityFramework.DAL.Extensions;
using production_supply_system.EntityFramework.DAL.LotContext;
using production_supply_system.EntityFramework.DAL.LotContext.Models;
using production_supply_system.EntityFramework.DAL.MasterProcessContext.Models;
using production_supply_system.EntityFramework.DAL.Models.dboSchema;
using production_supply_system.EntityFramework.DAL.Models.UsersSchema;


namespace BLL.Services
{
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
    public class StaticDataService(
        LotContext lotContext,
        MasterProcessContext masterProcessContext,
        ILogger<StaticDataService> logger) : IStaticDataService
    {

        #region Get by id

        /// <inheritdoc />
        public async Task<Process> GetProcessByIdAsync(int processId)
        {
            //try
            //{
            //    logger.LogTrace(string.Format(Resources.LogProcessGetById, processId));

            //    Process process = await processRepository.GetByIdAsync(processId);

            //    logger.LogTrace($"{string.Format(Resources.LogProcessGetById, processId)} {Resources.Completed}");

            //    return process;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogProcessGetById, processId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<Section> GetSectionByIdAsync(int sectionId)
        {
            //try
            //{
            //    logger.LogTrace(string.Format(Resources.LogSectionGetById, sectionId));

            //    Section section = await sectionRepository.GetByIdAsync(sectionId);

            //    logger.LogTrace($"{string.Format(Resources.LogSectionGetById, sectionId)} {Resources.Completed}");

            //    return section;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogSectionGetById, sectionId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<TypesOfContainer> GetContainerTypeByIdAsync(int containerTypeId)
        {
            //try
            //{
            //    logger.LogTrace(string.Format(Resources.LogTypesOfContainerGetById, containerTypeId));

            //    TypesOfContainer containerType = await containerTypeRepository.GetByIdAsync(containerTypeId);

            //    logger.LogTrace($"{string.Format(Resources.LogTypesOfContainerGetById, containerTypeId)} {Resources.Completed}");

            //    return containerType;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogTypesOfContainerGetById, containerTypeId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<TypesOfPart> GetPartTypeByIdAsync(int partTypeId)
        {
            //try
            //{
            //    logger.LogTrace(string.Format(Resources.LogTypesOfPartGetById, partTypeId));

            //    TypesOfPart partType = await partTypeRepository.GetByIdAsync(partTypeId);

            //    logger.LogTrace($"{string.Format(Resources.LogTypesOfPartGetById, partTypeId)} {Resources.Completed}");

            //    return partType;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogTypesOfPartGetById, partTypeId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<TypesOfPacking> GetPackingTypeByIdAsync(int packingTypeId)
        {
            //try
            //{
            //    logger.LogTrace(string.Format(Resources.LogTypesOfPackingGetById, packingTypeId));

            //    TypesOfPacking packingType = await packingTypesRepository.GetByIdAsync(packingTypeId);

            //    logger.LogTrace($"{string.Format(Resources.LogTypesOfPackingGetById, packingTypeId)} {Resources.Completed}");

            //    return packingType;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogTypesOfPackingGetById, packingTypeId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<TypesOfLocation> GetLocationTypeByIdAsync(int locationTypeId)
        {
            //try
            //{
            //    logger.LogTrace(string.Format(Resources.LogTypesOfLocationGetById, locationTypeId));

            //    TypesOfLocation locationType = await locationTypeRepository.GetByIdAsync(locationTypeId);

            //    logger.LogTrace($"{string.Format(Resources.LogTypesOfLocationGetById, locationTypeId)} {Resources.Completed}");

            //    return locationType;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogTypesOfLocationGetById, locationTypeId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<Carrier> GetCarrierByIdAsync(int carrierId)
        {
            //try
            //{
            //    logger.LogTrace(string.Format(Resources.LogCarrierGetById, carrierId));

            //    Carrier carrier = await carrierRepository.GetByIdAsync(carrierId);

            //    logger.LogTrace($"{string.Format(Resources.LogCarrierGetById, carrierId)} {Resources.Completed}");

            //    return carrier;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogCarrierGetById, carrierId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<TermsOfDelivery> GetDeliveryTermByIdAsync(int deliveryTermId)
        {
            //try
            //{
            //    logger.LogTrace(string.Format(Resources.LogTermsOfDeliveryGetById, deliveryTermId));

            //    TermsOfDelivery termsOfDelivery = await termsOfDeliveryRepository.GetByIdAsync(deliveryTermId);

            //    logger.LogTrace($"{string.Format(Resources.LogTermsOfDeliveryGetById, deliveryTermId)} {Resources.Completed}");

            //    return termsOfDelivery;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogTermsOfDeliveryGetById, deliveryTermId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<Location> GetLocationByIdAsync(int locationId)
        {
            //try
            //{
            //    logger.LogTrace(string.Format(Resources.LogLocationGetById, locationId));

            //    Location location = await locationRepository.GetByIdAsync(locationId);

            //    logger.LogTrace($"{string.Format(Resources.LogLocationGetById, locationId)} {Resources.Completed}");

            //    if (location is not null)
            //    {
            //        location.LocationType = await GetLocationTypeByIdAsync(location.LocationTypeId);
            //    }

            //    return location;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogLocationGetById, locationId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<Transport> GetTransportByIdAsync(int transportId)
        {
            //try
            //{
            //    logger.LogTrace(string.Format(Resources.LogTransportGetById, transportId));

            //    Transport transport = await transportRepository.GetByIdAsync(transportId);

            //    logger.LogTrace($"{string.Format(Resources.LogTransportGetById, transportId)} {Resources.Completed}");

            //    return transport;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogTransportGetById, transportId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<TypesOfTransport> GetTransportTypeByIdAsync(int transportTypeId)
        {
            //try
            //{
            //    logger.LogTrace(string.Format(Resources.LogTypesOfTransportGetById, transportTypeId));

            //    TypesOfTransport transportType = await transportTypeRepository.GetByIdAsync(transportTypeId);

            //    logger.LogTrace($"{string.Format(Resources.LogTypesOfTransportGetById, transportTypeId)} {Resources.Completed}");

            //    return transportType;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogTypesOfTransportGetById, transportTypeId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<Shipper> GetShipperByIdAsync(int shipperId)
        {
            //try
            //{
            //    logger.LogTrace(string.Format(Resources.LogShipperGetById, shipperId));

            //    Shipper shipper = await shipperRepository.GetByIdAsync(shipperId);

            //    logger.LogTrace($"{string.Format(Resources.LogShipperGetById, shipperId)} {Resources.Completed}");

            //    if (shipper is not null)
            //    {
            //        if (shipper.ShipperDefaultDeliveryLocationId is not null)
            //        {
            //            shipper.ShipperDefaultDeliveryLocation = await GetLocationByIdAsync((int)shipper.ShipperDefaultDeliveryLocationId);
            //        }
            //    }

            //    return shipper;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogShipperGetById, shipperId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<TypesOfOrder> GetPurchaseOrderTypeById(int orderTypeId)
        {
            //try
            //{
            //    logger.LogTrace(string.Format(Resources.LogTypesOfOrderGetById, orderTypeId));

            //    TypesOfOrder orderType = await orderTypesRepository.GetByIdAsync(orderTypeId);

            //    logger.LogTrace($"{string.Format(Resources.LogTypesOfOrderGetById, orderTypeId)} {Resources.Completed}");

            //    return orderType;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogTypesOfOrderGetById, orderTypeId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        #endregion Get by id

        #region Get All

        public async Task<IEnumerable<TypesOfPart>> GetAllPartTypesAsync()
        {
            //try
            //{
            //    logger.LogTrace(Resources.LogTypesOfPartGet);

            //    IEnumerable<TypesOfPart> partTypes = await partTypeRepository.GetAllAsync();

            //    logger.LogTrace($"{Resources.LogTypesOfPartGet} {Resources.Completed}");

            //    return partTypes;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogTypesOfPartGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TypesOfPacking>> GetAllPackingTypesAsync()
        {
            //try
            //{
            //    logger.LogTrace(Resources.LogTypesOfPackingGet);

            //    IEnumerable<TypesOfPacking> packingTypes = await packingTypesRepository.GetAllAsync();

            //    logger.LogTrace($"{Resources.LogTypesOfPackingGet} {Resources.Completed}");

            //    return packingTypes;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogTypesOfPackingGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<List<Shipper>> GetAllShippersAsync()
        {
            try
            {
                logger.LogTrace(Resources.LogShipperGet);

                List<Shipper> shippers = await lotContext.Shippers.ToListAsync();

                logger.LogTrace($"{Resources.LogShipperGet} {Resources.Completed}");

                return shippers;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogShipperGet}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<List<Carrier>> GetAllCarriersAsync()
        {
            try
            {
                logger.LogTrace(Resources.LogCarrierGet);

                List<Carrier> carriers = await lotContext.Carriers.ToListAsync();

                logger.LogTrace($"{Resources.LogCarrierGet} {Resources.Completed}");

                return carriers;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogCarrierGet}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<List<TermsOfDelivery>> GetAllTermsOfDeliveryAsync()
        {
            try
            {
                logger.LogTrace(Resources.LogTermsOfDeliveryGet);

                List<TermsOfDelivery> termsOfDelivery = await lotContext.TermsOfDeliveries.ToListAsync();

                logger.LogTrace($"{Resources.LogTermsOfDeliveryGet} {Resources.Completed}");

                return termsOfDelivery;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogTermsOfDeliveryGet}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<List<TypesOfTransport>> GetAllTransportTypesAsync()
        {
            try
            {
                logger.LogTrace(Resources.LogTypesOfTransportGet);

                List<TypesOfTransport> typesOfTransports = await lotContext.TypesOfTransports.ToListAsync();

                logger.LogTrace($"{Resources.LogTypesOfTransportGet} {Resources.Completed}");

                return typesOfTransports;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogTypesOfTransportGet}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            //try
            //{
            //    logger.LogTrace(Resources.LogLocationGet);

            //    IEnumerable<Location> locations = await locationRepository.GetAllAsync();

            //    logger.LogTrace($"{Resources.LogLocationGet} {Resources.Completed}");

            //    foreach (Location location in locations)
            //    {
            //        TypesOfLocation locationType = await GetLocationTypeByIdAsync(location.LocationTypeId);

            //        location.LocationType = locationType;
            //    }

            //    return locations;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogLocationGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<List<Transport>> GetAllTransportsAsync()
        {
            try
            {
                logger.LogTrace(Resources.LogTransportGet);

                List<Transport> transports = await lotContext.Transports.ToListAsync();

                logger.LogTrace($"{Resources.LogTransportGet} {Resources.Completed}");

                return transports;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogTransportGet}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TypesOfContainer>> GetAllContainerTypes()
        {
            //try
            //{
            //    logger.LogTrace(Resources.LogTypesOfContainerGet);

            //    IEnumerable<TypesOfContainer> typesOfContainers = await containerTypeRepository.GetAllAsync();

            //    logger.LogTrace($"{Resources.LogTypesOfContainerGet} {Resources.Completed}");

            //    return typesOfContainers;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogTypesOfContainerGet}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }

        #endregion Get All

        /// <inheritdoc />
        public async Task<TypesOfPacking> GetExistingPackingTypeByTypeAsync(string packingType)
        {
            try
            {
                logger.LogInformation(string.Format(Resources.LogTypesOfPackingGetByTypeName, packingType));

                TypesOfPacking result = await lotContext.TypesOfPackings.FirstOrDefaultAsync(c => c.SupplierPackingType == packingType);

                logger.LogInformation($"{string.Format(Resources.LogTypesOfPackingGetByTypeName, packingType)} {Resources.Completed}, {string.Format(Resources.LogWithResult, result)}");

                return result;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {string.Format(Resources.LogTypesOfPackingGetByTypeName, packingType)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<Transport> AddTransportAsync(Transport newTransport)
        {
            //CreateTransportParameters parameters = new(newTransport);

            //try
            //{
            //    logger.LogInformation($"{Resources.LogTransportAdd}: {JsonConvert.SerializeObject(newTransport)}");

            //    newTransport = await transportRepository.CreateAsync(newTransport, StoredProcedureInbound.AddNewTransport, parameters);

            //    logger.LogInformation($"{Resources.LogTransportAdd} {Resources.Completed}");
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {Resources.LogTransportAdd}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            //RefreshTransport();

            //return newTransport;

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<List<Location>> GetLocationsByTypeAsync(LocationType locationType)
        {
            try
            {
                string locationTypeName = EnumExtensions.GetDescription(locationType);

                logger.LogInformation($"{string.Format(Resources.LogLocationFilterByType, locationTypeName)}");

                List<Location> filteredLocations = await lotContext.Locations
                    .Where(location => location.LocationType.LocationType == locationTypeName)
                    .Include(location => location.LocationType)
                    .ToListAsync();

                logger.LogInformation($"{string.Format(Resources.LogLocationFilterByType, locationTypeName)} {Resources.Completed}");

                return filteredLocations;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {string.Format(Resources.LogLocationFilterByType, locationType)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<TypesOfContainer> GetContainerTypeByName(string containerTypeName)
        {
            try
            {
                logger.LogInformation($"{string.Format(Resources.LogTypesOfContainerGetByTypeName, containerTypeName)}");

                TypesOfContainer containerType = await lotContext.TypesOfContainers
                    .FirstOrDefaultAsync(c => c.ContainerType == containerTypeName);

                logger.LogInformation($"{string.Format(Resources.LogTypesOfContainerGetByTypeName, containerTypeName)} {Resources.Completed} {string.Format(Resources.LogWithResult, containerType)}");

                return containerType;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {string.Format(Resources.LogTypesOfContainerGetByTypeName, containerTypeName)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<Location> GetFinalLocationAsync()
        {
            //logger.LogInformation(Resources.LogLocationFinalGet);

            //Location result = (await GetAllLocationsAsync()).
            //    FirstOrDefault(l => l.LocationType.LocationType == EnumExtensions.GetDescription(LocationType.FinalLocation));

            //logger.LogInformation($"{Resources.LogLocationFinalGet} {Resources.Completed} {string.Format(Resources.LogWithResult, JsonConvert.SerializeObject(result))}");

            //return result;

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<TypesOfPart> GetPartTypeByNameAsync(PartTypes partType)
        {
            try
            {
                logger.LogInformation($"{string.Format(Resources.LogTypesOfPartGetByPartTypeName, partType)}");

                TypesOfPart result = await lotContext.TypesOfParts.FirstOrDefaultAsync(pt => pt.PartType == partType.ToString());

                logger.LogInformation($"{string.Format(Resources.LogTypesOfPartGetByPartTypeName, partType)} {Resources.Completed} {string.Format(Resources.LogWithResult, result)}");

                return result;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {string.Format(Resources.LogTypesOfPartGetByPartTypeName, partType)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ProcessesStep>> GetProcessStepsByUserAsync(User user)
        {
            //try
            //{
            //    logger.LogTrace($"{string.Format(Resources.LogProcessStepGetBySectionId, user.SectionId)}");

            //    List<ProcessesStep> steps = await masterProcessContext.ProcessesSteps.Where(c => c.SectionId == user.SectionId).ToListAsync();

            //    IEnumerable<ProcessesStep> steps1 = (await processStepsRepository.GetAllAsync())
            //        .Where(c => c.SectionId == user.SectionId);

            //    logger.LogTrace($"{string.Format(Resources.LogProcessStepGetBySectionId, user.SectionId)} {Resources.Completed}");

            //    return steps1;
            //}
            //catch (Exception ex)
            //{
            //    string message = $"{Resources.Error} {string.Format(Resources.LogProcessStepGetBySectionId, user.SectionId)}: {JsonConvert.SerializeObject(ex)}";

            //    logger.LogError(message);

            //    throw new Exception(message);
            //}

            throw new NotImplementedException();
        }
    }
}
