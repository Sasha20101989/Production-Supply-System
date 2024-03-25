using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;
using production_supply_system.EntityFramework.DAL.Enums;
using production_supply_system.EntityFramework.DAL.LotContext.Models;
using production_supply_system.EntityFramework.DAL.MasterProcessContext.Models;

namespace BLL.Contracts
{
    /// <summary>
    /// Интерфейс сервиса для операций, связанных с отгрузкой.
    /// </summary>
    public interface IDeliveryService
    {
        event EventHandler<ControllerDetails> DeliveryLoadProgressUpdated;

        /// <summary>
        /// Получает все заказы
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая все заказы.</returns>
        Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync();

        /// <summary>
        /// Загружает лоты в базу данных
        /// </summary>
        /// <param name="steps">Шаги которые необходимы для загрузки лота</param>
        /// <param name="lotDetails">Детали для лота</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая лот после загрузки в базу данных.</returns>
        Task<Lot> StartUploadLotAsync(List<ProcessesStep> steps, Lot lotDetails);

        /// <summary>
        /// Получает шаги для процесса
        /// </summary>
        /// <param name="appProcess">Значение пересичления типа процесса</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая шаги для процесса.</returns>
        Task<List<ProcessesStep>> GetProcessStepsByUserSectionAsync(AppProcess appProcess);

        /// <summary>
        /// Получает лот по уникальному идентификатору
        /// </summary>
        /// <param name="lotId">Уникальный идентификатор лота</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая лот по уникальному идентификатору.</returns>
        Task<Lot> GetLotByIdAsync(int lotId);

        /// <summary>
        /// Получает контейнеры по уникальному идентификатору лота
        /// </summary>
        /// <param name="lotId">Уникальный идентификатор лота</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая контейнеры по уникальному идентификатору лота.</returns>
        Task<List<ContainersInLot>> GetAllContainersByLotIdAsync(int lotId);

        /// <summary>
        /// Получает детали по уникальному идентификатору контейнера
        /// </summary>
        /// <param name="containerId">Уникальный идентификатор контейнера</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая детали по уникальному идентификатору контейнера.</returns>
        Task<List<PartsInContainer>> GetPartsForContainerAsync(int containerId);

        /// <summary>
        /// Получает все лоты
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая все лоты.</returns>
        Task<List<Lot>> GetAllLotsAsync();

        /// <summary>
        /// Получает количество контейнеров по уникальному идентификатору лота
        /// </summary>
        /// <param name="lotId">Уникальный идентификатор лота</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая количество контейнеров по уникальному идентификатору лота.</returns>
        Task<int> GetquantityContainersForLotId(int lotId);

        /// <summary>
        /// Получает весь трейсинг для выгрузки
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, выгружающая весь трейсинг для партнёра 2 для дальнейшей выгрузки в файл.</returns>
        Task<SheetData> GetAllTracingForPartner2ToExport(List<DocmapperContent> content);

        
        List<PurchaseOrder> GetPurchaseOrdersForShipper(Shipper shipper);
    }
}