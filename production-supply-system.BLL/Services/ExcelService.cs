using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using BLL.Contracts;

using DAL.Models.Docmapper;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace BLL.Services
{
    /// <summary>
    /// Сервис для работы с файлами Excel.
    /// </summary>
    public class ExcelService : IExcelService
    {
        private readonly ILogger<ExcelService> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ExcelService"/>.
        /// </summary>
        /// <param name="logger">Интерфейс для записи логов.</param>
        public ExcelService(ILogger<ExcelService> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public object[,] ReadExcelFile(string filePath, string sheetName)
        {
            try
            {
                _logger.LogInformation($"Начало парсинга файла '{filePath}' с листа '{sheetName}'.");

                using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, false);
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

                WorksheetPart worksheetPart = GetWorksheetPartByName(spreadsheetDocument, sheetName);

                if (worksheetPart != null)
                {
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                    int rowCount = sheetData.Elements<Row>().Count();

                    int colCount = sheetData
                        .Elements<Row>()
                        .Select(row => row.Elements<Cell>().LastOrDefault()?.CellReference?.Value.Length > 1 ?
                                       GetColumnIndexFromCellReference(row.Elements<Cell>().Last().CellReference.Value) :
                                       0)
                        .DefaultIfEmpty(0)
                        .Max();

                    object[,] dataArray = new object[rowCount, colCount];

                    int rowIdx = 0;

                    foreach (Row row in sheetData.Elements<Row>())
                    {
                        int colIdx = 0;

                        int lastColumnIndex = 0;

                        foreach (Cell cell in row.Elements<Cell>())
                        {
                            int currentColumnIndex = GetColumnIndexFromCellReference(cell.CellReference.Value);

                            // Заполняем пустые колонки, если есть смещение

                            for (int i = lastColumnIndex + 1; i < currentColumnIndex; i++)
                            {
                                dataArray[rowIdx, colIdx++] = null;
                            }

                            string cellValue = GetCellValue(cell, workbookPart);

                            dataArray[rowIdx, colIdx] = cellValue;

                            colIdx++;

                            lastColumnIndex = currentColumnIndex;
                        }

                        rowIdx++;
                    }

                    _logger.LogInformation($"Парсинг файла '{filePath}' с листа '{sheetName}' успешно завершён.");

                    return dataArray;
                }
                else
                {
                    string message = $"Лист '{sheetName}' не найден в документе '{filePath}'.";

                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                throw;
            }
        }

        /// <inheritdoc />
        public bool ValidateExcelDataHeaders(object[,] excelData, int firstRow, List<DocumentContent> content)
        {
            bool hasError = false;

            int row = firstRow - 1;

            _logger.LogInformation($"Начало валидации заголовков на строке №'{row}' в массиве данных '{JsonConvert.SerializeObject(excelData)}");

            content = content.OrderBy(item => item.ColumnNumber).ToList();

            for (int i = 0; i < content.Count; i++)
            {
                DocumentContent contentItem = content[i];

                if (contentItem.RowNumber is not null)
                {
                    continue;
                }

                object cell = excelData[row, contentItem.ColumnNumber - 1];

                if (cell is not null)
                {
                    if (cell.ToString() != contentItem.DocumentColumn.ElementName)
                    {
                        hasError = true;

                        break;
                    }
                }
            }

            _logger.LogInformation($"Валидация завершена с результатом '{!hasError}'.");

            return hasError;
        }

        private static int GetColumnIndexFromCellReference(string cellReference)
        {
            string columnName = Regex.Replace(cellReference, @"\d", string.Empty);

            int index = 0;

            int multiplier = 1;

            foreach (char c in columnName.Reverse())
            {
                index += multiplier * (c - 'A' + 1);

                multiplier *= 26;
            }

            return index;
        }

        /// <summary>
        /// Получает лист (Worksheet) по его имени.
        /// </summary>
        /// <param name="spreadsheetDocument">Документ Excel.</param>
        /// <param name="sheetName">Имя листа.</param>
        /// <returns>Объект WorksheetPart, представляющий лист.</returns>
        private WorksheetPart GetWorksheetPartByName(SpreadsheetDocument spreadsheetDocument, string sheetName)
        {
            _logger.LogInformation($"Поиск листа '{sheetName}' в документе.");

            IEnumerable<Sheet> sheets = spreadsheetDocument.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Any())
            {
                _logger.LogInformation($"Лист '{sheetName}' обнаружен.");

                string relationshipId = sheets.First().Id;

                return (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(relationshipId);
            }

            return null;
        }

        /// <summary>
        /// Получает значение ячейки в строке Excel.
        /// </summary>
        /// <param name="cell">Ячейка Excel.</param>
        /// <param name="workbookPart">Часть рабочей книги Excel.</param>
        /// <returns>Значение ячейки.</returns>
        private static string GetCellValue(Cell cell, WorkbookPart workbookPart)
        {
            SharedStringTablePart sharedStringTablePart = workbookPart.SharedStringTablePart;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                int sharedStringIndex = int.Parse(cell.InnerText);

                return sharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(sharedStringIndex).InnerText;
            }
            else
            {
                return cell.InnerText;
            }
        }
    }
}
