using System.Collections.Generic;
using System.Threading.Tasks;

using DAL.Models.Docmapper;

namespace DAL.Data.Contracts
{
    public interface IDocumentColumnData
    {
        /// <summary>
        /// Сбрасывает предыдущее состояние обьекта и загружает из источника
        /// </summary>
        void Refresh();

        /// <summary>
        /// Асинхронно получает список колонок документа.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую список колонок документов.</returns>
        Task<IEnumerable<DocumentColumn>> GetAllAsync();

        /// <summary>
        /// Получает колонку документа по его уникальному идентификатору
        /// </summary>
        /// <param name="docmapperColumnId">Уникальный идентификатор колонки документа</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая колонку документа по уникальному идентификатору</returns>
        Task<DocumentColumn> GetByIdAsync(int docmapperColumnId);

        /// <summary>
        /// Асинхронно добавляет элемент используемый в элементе контента документа
        /// </summary>
        /// <param name="documentColumn">Информация о элементе используемом в элементе контента документа</param>
        /// <returns>Задача, представляющая асинхронную операцию, добавляющую элемент используемый в элементе контента документа.</returns>
        Task<DocumentColumn> AddDocumentColumnAsync(DocumentColumn documentColumn);
    }
}