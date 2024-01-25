using System.Collections.Generic;
using System.Threading.Tasks;

using DAL.Models.Docmapper;

namespace DAL.Data.Contracts
{
    /// <summary>
    /// Интерфейс данных для операций, связанных с картами для документов эксель.
    /// </summary>
    public interface IDocumentData
    {
        /// <summary>
        /// Сбрасывает предыдущее состояние обьекта и загружает из источника
        /// </summary>
        void Refresh();

        /// <summary>
        /// Асинхронно получает список карт документов.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую список документов.</returns>
        Task<IEnumerable<Document>> GetAllAsync();

        /// <summary>
        /// Получает карту по ее уникальному идентификатору
        /// </summary>
        /// <param name="mapId">Уникальный идентификатор карты документа</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая документ по уникальному идентификатору</returns>
        Task<Document> GetDocumentByIdAsync(int mapId);

        /// <summary>
        /// Асинхронно создает документ.
        /// </summary>
        /// <param name="document"></param>
        /// <returns>Задача, представляющая асинхронную операцию, добавляющую документ и возвращающую документ с текущим Id.</returns>
        Task<Document> CreateDocumentAsync(Document document);

        /// <summary>
        /// Асинхронно обновляет документ.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, обновляющую документ.</returns>
        Task UpdateDocumentAsync(Document document);
    }
}