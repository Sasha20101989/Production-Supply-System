using System.Threading.Tasks;
using DAL.Models.BOM;

namespace BLL.Contracts
{
    /// <summary>
    /// Интерфейс сервиса для операций, связанных с BOM.
    /// </summary>
    public interface IBOMService
    {
        /// <summary>
        /// Добавляет детать и возвращает ее с уникальным идентификатором
        /// </summary>
        /// <param name="newBomPart"></param>
        /// <returns>Задача, представляющая асинхронную операцию, добавляющая детать и возвращающая ее с уникальным идентификатором</returns>
        Task<BomPart> SaveNewBomPartAsync(BomPart newBomPart);

        /// <summary>
        /// Возвращает деталь или null
        /// </summary>
        /// <param name="partNumber"></param>
        /// <returns>Задача, представляющая асинхронную операцию, которая возвращает детать по имени детали или Null если ее не найдёт</returns>
        Task<BomPart> GetExistingBomPartByPartNumberAsync(string partNumber);
    }
}
