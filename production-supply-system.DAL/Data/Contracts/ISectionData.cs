using System.Threading.Tasks;

using DAL.Models;

namespace DAL.Data.Contracts
{
    /// <summary>
    /// Интерфейс данных для операций, связанных с секциями.
    /// </summary>
    public interface ISectionData
    {
        /// <summary>
        /// Сбрасывает предыдущее состояние обьекта и загружает из источника
        /// </summary>
        void Refresh();

        /// <summary>
        /// Получает секцию по ее уникальному идентификатору
        /// </summary>
        /// <param name="sectionId">Уникальный идентификатор секции</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая секцию по уникальному идентификатору</returns>
        Task<Section> GetSectionByIdAsync(int sectionId); 
    }
}
