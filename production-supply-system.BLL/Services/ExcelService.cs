using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BLL.Contracts;
using ClosedXML.Excel;


using DAL.Models;
using DAL.Models.Document;

using DocumentFormat.OpenXml.Drawing;
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

        /// <inheritdoc />
        public void ColorCellsInDocument(Dictionary<string, CellInfo> validationErrors, string sheetName, string ngFilePath, string destinationFilePath)
        {
            _logger.LogInformation($"The beginning of filling in the cells in the document '{ngFilePath}' on sheet name '{sheetName}'.");

            using XLWorkbook workbook = new(destinationFilePath);
            XLWorkbook nGWorkbook = null;

            IXLWorksheet nGWorksheet = null;

            if (!File.Exists(ngFilePath))
            {
                workbook.SaveAs(ngFilePath);
            }

            nGWorkbook = new XLWorkbook(ngFilePath);

            nGWorksheet = nGWorkbook.Worksheet(sheetName);

            if (nGWorksheet is null)
            {
                throw new Exception($"Не найден лист '{sheetName}' для  того чтобы на нём отметить NG ячейки.");
            }

            foreach (KeyValuePair<string, CellInfo> validationError in validationErrors)
            {
                foreach (CustomError error in validationError.Value.Errors)
                {
                    HighlightCell(nGWorksheet, error.Row, error.Column, XLColor.Red);
                    AddCommentToCell(nGWorksheet, error.Row, error.Column, error.ErrorMessage);
                }
            }

            _logger.LogInformation($"Filling in the cells in the document '{ngFilePath}' on sheet name '{sheetName}' completed.");

            nGWorkbook.Save();
        }

        /// <inheritdoc />
        public void ExportFile<T>(IEnumerable<T> data, string filePath)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public object[,] ReadExcelFile(string filePath, string sheetName)
        {
            try
            {
                _logger.LogInformation($"The beginning of parsing a file '{filePath}' from a sheet '{sheetName}'.");

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

                    _logger.LogInformation($"Parsing a file '{filePath}' from a sheet '{sheetName}' completed.");

                    return dataArray;
                }
                else
                {
                    throw new Exception($"Sheet '{sheetName}' not found in the document '{filePath}'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                throw;
            }
        }

        /// <inheritdoc />
        public bool ValidateExcelDataHeaders(object[,] excelData, int firstRow, List<DocmapperContent> content)
        {
            bool hasError = false;

            int row = firstRow - 1;

            _logger.LogInformation($"The beginning of header validation on a line №'{firstRow}'");

            content = content.OrderBy(item => item.ColumnNr).ToList();

            for (int i = 0; i < content.Count; i++)
            {
                DocmapperContent contentItem = content[i];

                if (contentItem.RowNr is not null)
                {
                    continue;
                }

                object cell = excelData[row, contentItem.ColumnNr - 1];

                if (cell is not null)
                {
                    if (!string.Equals(cell?.ToString(), contentItem?.DocmapperColumn?.ElementName, StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogInformation($"Validation of headers in the file was completed with an error: in place of the expected '{contentItem?.DocmapperColumn?.ElementName}' is located '{cell?.ToString()}'");

                        throw new Exception($"Validation of headers in the file was completed with an error: in place of the expected '{contentItem?.DocmapperColumn?.ElementName}' is located '{cell?.ToString()}'");
                    }
                }
            }

            _logger.LogInformation($"Header validation on a line №'{firstRow}' completed.");

            return hasError;
        }

        /// <summary>
        /// Получает лист (Worksheet) по его имени.
        /// </summary>
        /// <param name="spreadsheetDocument">Документ Excel.</param>
        /// <param name="sheetName">Имя листа.</param>
        /// <returns>Объект WorksheetPart, представляющий лист.</returns>
        private WorksheetPart GetWorksheetPartByName(SpreadsheetDocument spreadsheetDocument, string sheetName)
        {
            _logger.LogInformation($"Search for the sheet '{sheetName}' in the document.");

            IEnumerable<Sheet> sheets = spreadsheetDocument.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Any())
            {
                _logger.LogInformation($"The sheet '{sheetName}' is detected.");

                string relationshipId = sheets.First().Id;

                return (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(relationshipId);
            }

            _logger.LogInformation($"The sheet '{sheetName}' is not detected.");

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

        /// <summary>
        /// Закрашивает ячейку
        /// </summary>
        /// <param name="worksheet">Лист документа</param>
        /// <param name="row">Строка</param>
        /// <param name="column">Колонка</param>
        /// <param name="color">Цвет</param>
        private void HighlightCell(IXLWorksheet worksheet, int row, int column, XLColor color)
        {
            _logger.LogInformation($"Starting to fill row '{row}' and column '{column}'");

            worksheet.Cell(row, column).Style.Fill.BackgroundColor = color;

            _logger.LogInformation($"Filling row '{row}' and column '{column}' completed.");
        }

        /// <summary>
        /// Добавляет выноску к ячейке с текстом
        /// </summary>
        /// <param name="worksheet">Лист документа</param>
        /// <param name="row">Строка</param>
        /// <param name="column">Колонка</param>
        /// <param name="commentText">Комментарий</param>
        private void AddCommentToCell(IXLWorksheet worksheet, int row, int column, string commentText)
        {
            _logger.LogInformation($"Starting to add comment '{commentText}' to row '{row}' and column '{column}'");

            IXLComment comment = worksheet.Cell(row, column).CreateComment();
            _ = comment.AddText(commentText);
            _ = comment.SetVisible();
            comment.FontSize = 10;

            _logger.LogInformation($"Adding comment '{commentText}' to row '{row}' and column '{column}' completed.");
        }
    }
}
