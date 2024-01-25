using System.Collections.Generic;
using System.Threading.Tasks;

using DAL.Models.Docmapper;

namespace DAL.Data.Contracts
{
    public interface IDocumentContentData
    {
        /// <summary>
        /// Сбрасывает предыдущее состояние обьекта и загружает из источника
        /// </summary>
        void Refresh();

        /// <summary>
        /// Получает контент карты по уникальному идентификатору карты
        /// </summary>
        /// <param name="mapId">Уникальный идентификатор карты документа</param>
        /// <returns>Задача, представляющая асинхронную операцию, dозвращающая контент карты по уникальному идентификатору карты</returns>
        Task<IEnumerable<DocumentContent>> GetAllDocumentContentItemsByIdAsync(int mapId);

        /// <summary>
        /// Асинхронно получает список контента для всех карт.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую список контента для всех карт.</returns>
        Task<IEnumerable<DocumentContent>> GetAllAsync();

        /// <summary>
        /// Асинхронно создает контент для карты документа.
        /// </summary>
        /// <param name="content">контенд карты документа</param>
        /// <returns>Задача, представляющая асинхронную операцию, добавляющую контент для карты документа.</returns>
        Task CreateDocumentContentAsync(DocumentContent content);

        /// <summary>
        /// Получает элемент контента по уникальному идентификатору контента
        /// </summary>
        /// <param name="docmapperContentId">Уникальномый идентификатор контента</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую элемент контента по уникальному идентификатору контента.</returns>
        Task<DocumentContent> GetByIdAsync(int docmapperContentId);

        /// <summary>
        /// Асинхронно проверяет наличие контента
        /// </summary>
        /// <param name="content">контенд карты документа</param>
        /// <returns>Задача, представляющая асинхронную операцию, проверяющую наличие контента и возвращающую булево значение</returns>
        Task<bool> ExistsAsync(DocumentContent content);

        /// <summary>
        /// Асинхронно удаляет элемент контента по уникальному идентификатору контента
        /// </summary>
        /// <param name="docmapperContentId">Уникальномый идентификатор контента</param>
        /// <returns>Задача, представляющая асинхронную операцию, удаляющую элемент контента по уникальному идентификатору контента.</returns>
        Task DeleteAsync(int docmapperContentId);

        /// <summary>
        /// Асинхронно обновляет элемент контента
        /// </summary>
        /// <param name="content">контенд карты документа</param>
        /// <returns>Задача, представляющая асинхронную операцию, обновляющую элемент контента.</returns>
        Task UpdateAsync(DocumentContent content);
    }
}
