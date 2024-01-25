using System.Collections.Generic;
using System.Threading.Tasks;

using DAL.Models;

namespace DAL.Data.Contracts
{
    /// <summary>
    /// Интерфейс данных для операций, связанных с лотами.
    /// </summary>
    public interface ILotData
    {
        /// <summary>
        /// Асинхронно получает список лотов.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую список лотов.</returns>
        Task<IEnumerable<Lot>> GetAllLotsAsync();
    }
}
