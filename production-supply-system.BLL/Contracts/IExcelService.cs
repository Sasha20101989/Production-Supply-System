using DAL.Models.Docmapper;
using System.Collections.Generic;

namespace BLL.Contracts
{
    /// <summary>
    /// Интерфейс сервиса для операций, связанных с excel.
    /// </summary>
    public interface IExcelService
    {
        /// <summary>
        /// Читает файл эксель
        /// </summary>
        /// <param name="filePath">Путь к документу</param>
        /// <param name="sheetName">Лист документа</param>
        /// <param name="targetRowIndex">Не обязательный параметр, указывающий на строку с заголовками табличной части документа</param>
        /// <returns></returns>
        object[,] ReadExcelFile(string filePath, string sheetName);

        /// <summary>
        /// Валидирует заголовки табличной части документа, что они совпадают с картой документов
        /// </summary>
        /// <param name="excelData">Массив с данными из файла эксель</param>
        /// <param name="firstRow">Строка с заголовками</param>
        /// <param name="content">Список колонок</param>
        /// <returns>Метод возвращающий результат валидации, если заголовки валидны то true, иначе false</returns>
        bool ValidateExcelDataHeaders(object[,] excelData, int firstRow, List<DocumentContent> content);
    }
}
