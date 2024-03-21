using System.Collections.Generic;

using DAL.Models;
using DAL.Models.Document;

using DocumentFormat.OpenXml.Spreadsheet;

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
        /// <returns></returns>
        object[,] ReadExcelFile(string filePath, string sheetName);

        /// <summary>
        /// Валидирует заголовки табличной части документа, что они совпадают с картой документов
        /// </summary>
        /// <param name="firstRow">Строка с заголовками</param>
        /// <param name="content">Список колонок</param>
        /// <param name="filePath">Путь к документу</param>
        /// <param name="sheetName">Лист документа</param>
        /// <returns>Метод возвращающий результат валидации, если заголовки валидны то возвращает массив с данными, иначе null</returns>
        object[,] ValidateExcelDataHeaders(int firstRow, List<DocmapperContent> content, string filePath, string sheetName);

        /// <summary>
        /// Красит ячейки в документе
        /// </summary>
        /// <param name="validationErrors">Список ошибок</param>
        /// <param name="sheetName">Имя листа</param>
        /// <param name="ngFilePath">Путь к файлу с ошибками</param>
        /// <param name="destinationFilePath">Путь к оригинальному файлу</param>
        void ColorCellsInDocument(Dictionary<string, CellInfo> validationErrors, string sheetName, string ngFilePath, string destinationFilePath);

        /// <summary>
        /// Выгружает данные в файл
        /// </summary>
        /// <example>
        /// Пример использования:
        /// <code>
        /// SheetData sheetData = new();
        /// 
        /// Row headerRow = new();
        /// 
        /// foreach (DocmapperContent item in orderedByColumnContent)
        /// {
        ///     Cell cell = new();
        ///     cell.DataType = CellValues.String;
        ///     cell.CellValue = new CellValue(item.DocmapperColumn.ElementName);
        ///     headerRow.Append(cell.CellValue);
        /// }
        /// 
        /// sheetData.Append(headerRow);
        /// </code>
        /// </example>
        /// <param name="data">Содержимое</param>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="sheetName">Имя листа</param>
        void ExportFile(SheetData data, string filePath, string sheetName);
    }
}
