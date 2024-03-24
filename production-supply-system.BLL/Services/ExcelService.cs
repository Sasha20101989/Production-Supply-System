using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using BLL.Contracts;
using BLL.Properties;

using ClosedXML.Excel;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;

namespace BLL.Services
{
    /// <summary>
    /// Сервис для работы с файлами Excel.
    /// </summary>
    /// <remarks>
    /// Инициализирует новый экземпляр класса <see cref="ExcelService"/>.
    /// </remarks>
    /// <param name="logger">Интерфейс для записи логов.</param>
    public partial class ExcelService(ILogger<ExcelService> logger) : IExcelService
    {
        private static int GetColumnIndexFromCellReference(string cellReference)
        {
            string columnName = ColumnRegex().Replace(cellReference, string.Empty);

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
            logger.LogInformation($"{string.Format(Resources.LogExcelFillCellsInDocument, ngFilePath, sheetName)}");

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
                throw new Exception($"{string.Format(Resources.LogExcelSheetNotFound, sheetName)}");
            }

            foreach (KeyValuePair<string, CellInfo> validationError in validationErrors)
            {
                foreach (CustomError error in validationError.Value.Errors)
                {
                    HighlightCell(nGWorksheet, error.Row, error.Column, XLColor.Red);
                    AddCommentToCell(nGWorksheet, error.Row, error.Column, error.ErrorMessage);
                }
            }

            logger.LogInformation($"{string.Format(Resources.LogExcelFillCellsInDocument, ngFilePath, sheetName)} {Resources.Completed}");

            nGWorkbook.Save();
        }

        /// <inheritdoc />
        public void ExportFile(SheetData data, string filePath, string sheetName)
        {
            using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);
            WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new();

            worksheetPart.Worksheet.Append(data);

            Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

            _ = sheets.AppendChild(new Sheet
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = sheetName
            });

            workbookPart.Workbook.Save();
        }

        /// <inheritdoc />
        public object[,] ReadExcelFile(string filePath, string sheetName)
        {
            try
            {
                logger.LogInformation($"{string.Format(Resources.LogExcelParsingFileFromSheet, filePath, sheetName)}");

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

                    logger.LogInformation($"{string.Format(Resources.LogExcelParsingFileFromSheet, filePath, sheetName)} {Resources.Completed}");

                    return dataArray;
                }
                else
                {
                    throw new Exception($"{string.Format(Resources.LogExcelSheetNotFound, sheetName)}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"{Resources.Error} {string.Format(Resources.LogExcelParsingFileFromSheet, filePath, sheetName)}: {JsonConvert.SerializeObject(ex)}");

                throw;
            }
        }

        /// <inheritdoc />
        public object[,] ValidateExcelDataHeaders(int firstRow, List<DocmapperContent> content, string folder, string sheetName)
        {
            bool hasError = false;

            int row = firstRow - 1;

            logger.LogInformation($"{string.Format(Resources.LogExcelHeaderValidation, firstRow)}");

            content = [.. content.OrderBy(item => item.ColumnNr)];

            object[,] excelData = ReadExcelFile(folder, sheetName);

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
                        string message = $"{Resources.Error} {string.Format(Resources.LogExcelHeaderValidation, firstRow)}: {string.Format(Resources.LogExceptionExpectedPlace, contentItem?.DocmapperColumn?.ElementName, cell?.ToString())}";

                        logger.LogError(message);

                        throw new Exception(message);
                    }
                }
            }

            logger.LogInformation($"{string.Format(Resources.LogExcelHeaderValidation, firstRow)} {Resources.Completed}");

            return hasError ? null : excelData;
        }

        /// <summary>
        /// Получает лист (Worksheet) по его имени.
        /// </summary>
        /// <param name="spreadsheetDocument">Документ Excel.</param>
        /// <param name="sheetName">Имя листа.</param>
        /// <returns>Объект WorksheetPart, представляющий лист.</returns>
        private WorksheetPart GetWorksheetPartByName(SpreadsheetDocument spreadsheetDocument, string sheetName)
        {
            logger.LogInformation($"{string.Format(Resources.LogExcelSheetSearch, sheetName)}");

            IEnumerable<Sheet> sheets = spreadsheetDocument.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Any())
            {
                logger.LogInformation($"{string.Format(Resources.LogExcelSheetSearch, sheetName)} {Resources.Completed}");

                string relationshipId = sheets.First().Id;

                return (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(relationshipId);
            }

            logger.LogError($"{string.Format(Resources.LogExcelSheetNotFound, sheetName)}");

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
            logger.LogInformation($"{string.Format(Resources.LogExcelFillRowAndColumn, row, column)}");

            worksheet.Cell(row, column).Style.Fill.BackgroundColor = color;

            logger.LogInformation($"{string.Format(Resources.LogExcelFillRowAndColumn, row, column)} {Resources.Completed}");
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
            logger.LogInformation($"{string.Format(Resources.LogExcelAddCommentAndFillRowAndColumn, commentText, row, column)}");

            IXLComment comment = worksheet.Cell(row, column).CreateComment();
            _ = comment.AddText(commentText);
            _ = comment.SetVisible();
            comment.FontSize = 10;

            logger.LogInformation($"{string.Format(Resources.LogExcelAddCommentAndFillRowAndColumn, commentText, row, column)} {Resources.Completed}");
        }

        [GeneratedRegex(@"\d")]
        private static partial Regex ColumnRegex();
    }
}
