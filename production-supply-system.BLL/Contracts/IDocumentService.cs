using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models.Document;

namespace BLL.Contracts
{
    /// <summary>
    /// Интерфейс сервиса для операций, связанных с картами сопоставления данных в экселе.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Асинхронно создает документ.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="documentContent"></param>
        /// <returns>Задача, представляющая асинхронную операцию, добавляющую документ и возвращающую документ с текущим Id.</returns>
        Task<Docmapper> CreateDocumentAsync(Docmapper document, List<DocmapperContent> documentContent);

        /// <summary>
        /// Асинхронно получает список карт документов.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую список документов.</returns>
        Task<IEnumerable<Docmapper>> GetAllAsync();

        /// <summary>
        /// Получает карту по ее уникальному идентификатору
        /// </summary>
        /// <param name="mapId">Уникальный идентификатор карты документа</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая документ по уникальному идентификатору</returns>
        Task<Docmapper> GetDocumentByIdAsync(int mapId);

        /// <summary>
        /// Асинхронно получает список колонок документа.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую список колонок документов.</returns>
        Task<IEnumerable<DocmapperColumn>> GetAllColumnsAsync();

        /// <summary>
        /// Асинхронно получает список контента документа.
        /// </summary>
        /// <param name="mapId">Уникальный идентификатор карты документа</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую список контента документа.</returns>
        Task<IEnumerable<DocmapperContent>> GetAllDocumentContentItemsByIdAsync(int mapId);

        /// <summary>
        /// Асинхронно обновляет документ.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="documentContent"></param>
        /// <returns>Задача, представляющая асинхронную операцию, обновляющую документ.</returns>
        Task UpdateDocumentAsync(Docmapper document, List<DocmapperContent> documentContent);

        /// <summary>
        /// Асинхронно добавляет элемент используемый в элементе контента документа
        /// </summary>
        /// <param name="documentColumn">Информация о элементе используемом в элементе контента документа</param>
        /// <returns>Задача, представляющая асинхронную операцию, добавляющую элемент используемый в элементе контента документа.</returns>
        Task<DocmapperColumn> AddDocumentColumnAsync(DocmapperColumn documentColumn);
    }
}
