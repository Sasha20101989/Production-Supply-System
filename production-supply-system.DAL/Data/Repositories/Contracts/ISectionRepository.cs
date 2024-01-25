using System.Threading.Tasks;

using DAL.Models;

namespace DAL.Data.Repositories.Contracts
{
    /// <summary>
    /// Репозиторий, отвечающий за доступ к информации о секции.
    /// </summary>
    public interface ISectionRepository
    {
        /// <summary>
        /// Получает секцию по ее уникальному идентификатору
        /// </summary>
        /// <param name="sectionId">Уникальный идентификатор секции</param>
        /// <returns>>Задача, представляющая асинхронную операцию, возвращающая секцию по уникальному идентификатору</returns>
        Task<Section> GetSectionByIdAsync(int sectionId);
    }
}
