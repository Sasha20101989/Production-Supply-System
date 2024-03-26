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
        /// Получает все лоты
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая все лоты.</returns>
        Task<List<Lot>> GetAllLotsAsync();

        /// <summary>
        /// Получает весь трейсинг для выгрузки
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, выгружающая весь трейсинг для партнёра 2 для дальнейшей выгрузки в файл.</returns>
        Task<SheetData> GetAllTracingForPartner2ToExportAsync(List<DocmapperContent> content);

        
        List<PurchaseOrder> GetPurchaseOrdersForShipper(Shipper shipper);
    }
}