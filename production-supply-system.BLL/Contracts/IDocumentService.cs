using System.Collections.Generic;
using System.Threading.Tasks;

using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;

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
        Task CreateDocumentAsync(Docmapper document);

        /// <summary>
        /// Асинхронно получает список карт документов.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую список документов.</returns>
        Task<List<Docmapper>> GetAllDocumentsAsync();

        /// <summary>
        /// Асинхронно получает список карт документов.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую список документов.</returns>
        Task<List<Docmapper>> GetFilteredDocumentsAsync(string docmapperName);

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
        /// Асинхронно обновляет документ.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="documentContent"></param>
        /// <returns>Задача, представляющая асинхронную операцию, обновляющую документ.</returns>
        Task UpdateDocumentAsync(Docmapper document);

        /// <summary>
        /// Асинхронно добавляет элемент используемый в элементе контента документа
        /// </summary>
        /// <param name="documentColumn">Информация о элементе используемом в элементе контента документа</param>
        /// <returns>Задача, представляющая асинхронную операцию, добавляющую элемент используемый в элементе контента документа.</returns>
        Task AddDocumentColumnAsync(DocmapperColumn documentColumn);

        Task UpdateDocumentContentsAsync(ICollection<DocmapperContent> docmapperContents, int docmapperId);
    }
}
