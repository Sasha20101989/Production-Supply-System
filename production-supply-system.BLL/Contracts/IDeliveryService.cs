using System.Collections.Generic;
using System.Threading.Tasks;

using DAL.Models;

namespace BLL.Contracts
{
    /// <summary>
    /// Интерфейс сервиса для операций, связанных с отгрузкой.
    /// </summary>
    public interface IDeliveryService
    {
        /// <summary>
        /// Асинхронно получает информацию о лотах
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую информацию о лотах.</returns>
        Task<IEnumerable<Lot>> GetAllLotItemsAsync();
    }
}