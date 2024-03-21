using System.Collections.Generic;
using System.Threading.Tasks;

using DAL.Enums;
using DAL.Models;
using DAL.Models.Master;
using DAL.Models.Partscontrol;

namespace BLL.Contracts
{
    /// <summary>
    /// Интерфейс сервиса для операций, связанных со статическими данными.
    /// </summary>
    public interface IStaticDataService
    {
        /// <summary>
        /// Получает всех отправителей
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая всех отправителей</returns>
        Task<IEnumerable<Shipper>> GetAllShippersAsync();

        /// <summary>
        /// Получает всех перевозчиков
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая всех перевозчиков</returns>
        Task<IEnumerable<Carrier>> GetAllCarriersAsync();

        /// <summary>
        /// Получает все варианты условий поставки
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая все варианты условий поставки.</returns>
        Task<IEnumerable<TermsOfDelivery>> GetAllTermsOfDeliveryAsync();

        /// <summary>
        /// Получает все типы транспорта
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая все типы транспорта.</returns>
        Task<IEnumerable<TypesOfTransport>> GetAllTransportTypesAsync();

        /// <summary>
        /// Получает все локации
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая все локации.</returns>
        Task<IEnumerable<Location>> GetAllLocationsAsync();

        /// <summary>
        /// Получает весь транспорт
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая весь транспорт.</returns>
        Task<IEnumerable<Transport>> GetAllTransportsAsync();

        /// <summary>
        /// Добавляет новый транспорт
        /// </summary>
        /// <param name="newTransport">Новый транспорт</param>
        /// <returns>Задача, представляющая асинхронную операцию, добавляющая и возвращающая новый транспорт.</returns>
        Task<Transport> AddTransportAsync(Transport newTransport);

        /// <summary>
        /// Возвращает локации по типу локации
        /// </summary>
        /// <param name="locationType">Тип локации</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая локации по типу локации.</returns>
        Task<IEnumerable<Location>> GetLocationsByTypeAsync(LocationType locationType);

        /// <summary>
        /// Возвращает тип контейнера по имени типа контейнера
        /// </summary>
        /// <param name="containerTypeName">Название типа контейнера</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая тип контейнера по имени типа контейнера.</returns>
        Task<TypesOfContainer> GetContainerTypeByName(string containerTypeName);

        /// <summary>
        /// Получает все типы контейнеров
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая все типы контейнеров.</returns>
        Task<IEnumerable<TypesOfContainer>> GetAllContainerTypes();

        /// <summary>
        /// Получает финальную локацию
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая финальную локацию.</returns>
        Task<Location> GetFinalLocationAsync();

        /// <summary>
        /// Получает перевозчика по уникальному идентификатору
        /// </summary>
        /// <param name="carrierId">Уникальный идентификатор перевозчика</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая перевозчика по уникальному идентификатору</returns>
        Task<Carrier> GetCarrierByIdAsync(int carrierId);

        /// <summary>
        /// Получает условие поставки по уникальному идентификатору
        /// </summary>
        /// <param name="deliveryTermId">Уникальный идентификатор поставки</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая условие поставки по уникальному идентификатору.</returns>
        Task<TermsOfDelivery> GetDeliveryTermByIdAsync(int deliveryTermId);

        /// <summary>
        /// Получает локацию по уникальному идентификатору
        /// </summary>
        /// <param name="locationId">Уникальный идентификатор локации</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая локацию по уникальному идентификатору.</returns>
        Task<Location> GetLocationByIdAsync(int locationId);

        /// <summary>
        /// Получает транспорт по уникальному идентификатору
        /// </summary>
        /// <param name="transportId">Уникальный идентификатор транспорта</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая транспорт по уникальному идентификатору.</returns>
        Task<Transport> GetTransportByIdAsync(int transportId);

        /// <summary>
        /// Получает тип транспорта по уникальному идентификатору
        /// </summary>
        /// <param name="transportTypeId">Уникальный идентификатор типа транспорта</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая тип транспорта по уникальному идентификатору.</returns>
        Task<TypesOfTransport> GetTransportTypeByIdAsync(int transportTypeId);

        /// <summary>
        /// Получает отправителя по уникальному идентификатору
        /// </summary>
        /// <param name="shipperId">Уникальный идентификатор отправителя</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая отправителя по уникальному идентификатору.</returns>
        Task<Shipper> GetShipperByIdAsync(int shipperId);

        /// <summary>
        /// Получает тип заказа по уникальному идентификатору
        /// </summary>
        /// <param name="orderTypeId">Уникальный идентификатор типа заказа</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая тип заказа по уникальному идентификатору.</returns>
        Task<TypesOfOrder> GetPurchaseOrderTypeById(int orderTypeId);

        /// <summary>
        /// Получает тип локации по уникальному идентификатору
        /// </summary>
        /// <param name="locationTypeId">Уникальный идентификатор типа локации</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая тип локации по уникальному идентификатору.</returns>
        Task<TypesOfLocation> GetLocationTypeByIdAsync(int locationTypeId);

        /// <summary>
        /// Получает тип упаковки по уникальному идентификатору
        /// </summary>
        /// <param name="packingTypeId">Уникальный идентификатор типа упаковки</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая тип упаковки по уникальному идентификатору.</returns>
        Task<TypesOfPacking> GetPackingTypeByIdAsync(int packingTypeId);

        /// <summary>
        /// Получает все типы упаковки
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая все типы упаковки.</returns>
        Task<IEnumerable<TypesOfPacking>> GetAllPackingTypesAsync();

        /// <summary>
        /// Получает существующий тип упаковки
        /// </summary>
        /// <param name="packingType">Тип упаковки</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая тип упаковки или null в случае отсутствия.</returns>
        Task<TypesOfPacking> GetExistingPackingTypeByTypeAsync(string packingType);

        /// <summary>
        /// Получает все типы детали
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая все типы детали.</returns>
        Task<IEnumerable<TypesOfPart>> GetAllPartTypesAsync();

        /// <summary>
        /// Получает тип детали по уникальному идентификатору
        /// </summary>
        /// <param name="partTypeId">Уникальный идентификатор типа детали</param>
        /// <returns>>Задача, представляющая асинхронную операцию, возвращающая тип детали.</returns>
        Task<TypesOfPart> GetPartTypeByIdAsync(int partTypeId);

        /// <summary>
        /// Получает тип контейнера по уникальному идентификатору
        /// </summary>
        /// <param name="containerTypeId">Уникальный идентификатор типа контейнера</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая тип контейнера по уникальному идентификатору.</returns>
        Task<TypesOfContainer> GetContainerTypeByIdAsync(int containerTypeId);

        /// <summary>
        /// Получает тип детали по имени типа
        /// </summary>
        /// <param name="partType">значение перечисления типа детали</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая тип детали или null в случае отсутствия.</returns>
        Task<TypesOfPart> GetPartTypeByNameAsync(PartTypes partType);

        /// <summary>
        /// Получает процессы для пользователя
        /// </summary>
        /// <param name="user">Пользователь для которого будут подобраны процессы</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая процессы для пользователя по его секции.</returns>
        Task<IEnumerable<ProcessStep>> GetProcessStepsByUserAsync(User user);

        /// <summary>
        /// Получает секцию по уникальному идентификатору
        /// </summary>
        /// <param name="sectionId">Уникальный идентификатор секции</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая секцию по уникальному идентификатору.</returns>
        Task<Section> GetSectionByIdAsync(int sectionId);

        /// <summary>
        /// Получает процесс по уникальному идентификатору
        /// </summary>
        /// <param name="processId">Уникальный идентификатор процесса</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая процесс по уникальному идентификатору.</returns>
        Task<Process> GetProcessByIdAsync(int processId);
    }
}
