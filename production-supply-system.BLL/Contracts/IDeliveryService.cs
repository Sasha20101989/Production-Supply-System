using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BLL.Models;

using DAL.Enums;
using DAL.Models;
using DAL.Models.Master;

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


        Task<Lot> StartProcessAsync(List<ProcessStep> steps, Lot lotDetails);


        Task<List<ProcessStep>> GetProcessStepsByUserSectionAsync(AppProcess appProcess);

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
        /// Получает колличество контейнеров по уникальному идентификатору лота
        /// </summary>
        /// <param name="lotId">Уникальный идентификатор лота</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая колличество контейнеров по уникальному идентификатору лота.</returns>
        Task<int> GetquantityContainersForLotId(int lotId);

        /// <summary>
        /// Выгружает все лоты в файл
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, выгружающая все лоты в файл.</returns>
        Task ExportAllTracing(string exportFilePath);
    }
}