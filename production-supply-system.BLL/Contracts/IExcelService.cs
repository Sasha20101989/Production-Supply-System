using System.Collections.Generic;

using DAL.Models;
using DAL.Models.Document;

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
        bool ValidateExcelDataHeaders(object[,] excelData, int firstRow, List<DocmapperContent> content);

        /// <summary>
        /// Красит ячейки в документе
        /// </summary>
        /// <param name="validationErrors">Список ошибок</param>
        /// <param name="sheetName">Имя листа</param>
        /// <param name="ngFilePath">Путь к файлу с ошибками</param>
        /// <param name="destinationFilePath">Путь к оригинальному файлу</param>
        void ColorCellsInDocument(Dictionary<string, CellInfo> validationErrors, string sheetName, string ngFilePath, string destinationFilePath);

        void ExportFile<T>(IEnumerable<T> data, string filePath);
    }
}
