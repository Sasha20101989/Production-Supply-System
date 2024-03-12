using System;
using System.Collections.Generic;
using System.IO;
using BLL.Services;

using DAL.Models.Document;

using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace production_supply_system.TEST.BLL.Services
{
    public class ExcelServiceTests
    {
        private readonly string assetsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");

        [Fact]
        public void ReadExcelFile_WhenValidFilePathAndSheetName_ReturnsNonEmptyArray()
        {
            // Arrange

            ExcelService excelService = new(new NullLogger<ExcelService>());

            string filePath = Path.Combine(assetsFolderPath, "validFilePath.xlsx");

            string sheetName = "Sheet1";

            // Act

            object[,] result = excelService.ReadExcelFile(filePath, sheetName);

            // Assert

            Assert.NotNull(result);

            Assert.True(result.Length > 0);
        }

        [Fact]
        public void ReadExcelFile_WhenInvalidSheetName_ThrowsException()
        {
            // Arrange

            ExcelService excelService = new(new NullLogger<ExcelService>());

            string filePath = Path.Combine(assetsFolderPath, "validFilePath.xlsx");

            string invalidSheetName = "InvalidSheet";

            // Act & Assert

            Exception ex = Assert.Throws<Exception>(() => excelService.ReadExcelFile(filePath, invalidSheetName));

            Assert.Equal($"Лист '{invalidSheetName}' не найден в документе '{filePath}'.", ex.Message);
        }

        [Fact]
        public void ValidateExcelDataHeaders_WhenValidData_ReturnsFalse()
        {
            // Arrange

            ExcelService excelService = new(new NullLogger<ExcelService>());

            object[,] excelData = new object[,] { { "Header1", "Header2" }, { "Value1", "Value2" } };

            int firstRow = 1;

            List<DocmapperContent> content = new()
            {
                new DocmapperContent { DocmapperColumn = new DocmapperColumn { ElementName = "Header1" }, ColumnNr = 1 },

                new DocmapperContent { DocmapperColumn = new DocmapperColumn { ElementName = "Header2" }, ColumnNr = 2 }
            };

            // Act

            bool result = excelService.ValidateExcelDataHeaders(excelData, firstRow, content);

            // Assert

            Assert.False(result);
        }

        [Fact]
        public void ValidateExcelDataHeaders_WhenInvalidData_ReturnsTrue()
        {
            // Arrange

            ExcelService excelService = new(new NullLogger<ExcelService>());

            object[,] excelData = new object[,] { { "InvalidHeader1", "Header2" }, { "Value1", "Value2" } };

            int firstRow = 1;

            List<DocmapperContent> content = new()
            {
                new DocmapperContent { DocmapperColumn = new DocmapperColumn { ElementName = "Header1" }, ColumnNr = 1 },

                new DocmapperContent { DocmapperColumn = new DocmapperColumn { ElementName = "Header2" }, ColumnNr = 2 }
            };

            // Act

            bool result = excelService.ValidateExcelDataHeaders(excelData, firstRow, content);

            // Assert

            Assert.True(result);
        }
    }
}
