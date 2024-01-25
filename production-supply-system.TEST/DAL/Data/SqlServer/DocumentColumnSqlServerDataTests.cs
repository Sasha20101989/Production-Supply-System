using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL.Data.SqlServer;
using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models.Docmapper;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace production_supply_system.TEST.DAL.Data.SqlServer
{
    public class DocumentColumnSqlServerDataTests
    {
        [Fact]
        public async Task GetByIdAsync_ExistingColumn_ReturnsColumn()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentColumnSqlServerData>> mockLogger = new();

            DocumentColumnSqlServerData documentColumnSqlServerData = new(mockDb.Object, mockLogger.Object);

            int existingId = 1;

            DocumentColumn existingColumn = new() { DocmapperColumnId = 1, ElementName = "Column1", SystemColumnName = "SystemColumn1" };

            List<DocumentColumn> expectedColumns = new() { existingColumn };

            _ = mockDb.Setup(db => db.LoadData<DocumentColumn>(StoredProcedureDocmapper.GetAllDocmapperColumns, It.IsAny<object>(), "Default"))
                .ReturnsAsync(expectedColumns);

            // Act

            DocumentColumn result = await documentColumnSqlServerData.GetByIdAsync(existingColumn.DocmapperColumnId);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(expectedColumns.FirstOrDefault(d => d.DocmapperColumnId == existingId), result);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingColumn_ReturnsNull()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentColumnSqlServerData>> mockLogger = new();

            DocumentColumnSqlServerData documentColumnSqlServerData = new(mockDb.Object, mockLogger.Object);

            int nonExistingId = 99;

            DocumentColumn existingColumn = new() { DocmapperColumnId = 1, ElementName = "Column1", SystemColumnName = "SystemColumn1" };

            List<DocumentColumn> expectedColumns = new() { existingColumn };

            _ = mockDb.Setup(db => db.LoadData<DocumentColumn>(StoredProcedureDocmapper.GetAllDocmapperColumns, It.IsAny<object>(), "Default"))
                .ReturnsAsync(expectedColumns);

            // Act

            DocumentColumn result = await documentColumnSqlServerData.GetByIdAsync(nonExistingId);

            // Assert

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllColumns()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentColumnSqlServerData>> mockLogger = new();

            DocumentColumnSqlServerData documentColumnSqlServerData = new(mockDb.Object, mockLogger.Object);

            List<DocumentColumn> expectedColumns = new()
            {
                new DocumentColumn { DocmapperColumnId = 1, ElementName = "Column1", SystemColumnName = "SystemColumn1" },
                new DocumentColumn { DocmapperColumnId = 2, ElementName = "Column2", SystemColumnName = "SystemColumn2" }
            };

            _ = mockDb.Setup(db => db.LoadData<DocumentColumn>(StoredProcedureDocmapper.GetAllDocmapperColumns, It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(expectedColumns);

            // Act

            IEnumerable<DocumentColumn> result = await documentColumnSqlServerData.GetAllAsync();

            // Assert

            Assert.NotNull(result);

            Assert.Equal(expectedColumns, result);
        }

        [Fact]

        public async Task AddDocumentColumnAsync_ValidColumn_ReturnsCreatedColumn()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentColumnSqlServerData>> mockLogger = new();

            DocumentColumnSqlServerData documentColumnSqlServerData = new(mockDb.Object, mockLogger.Object);

            DocumentColumn columnToAdd = new() { ElementName = "NewColumn", SystemColumnName = "NewSystemColumn" };

            DocumentColumn expectedColumn = new() { DocmapperColumnId = 1, ElementName = "NewColumn", SystemColumnName = "NewSystemColumn" };

            _ = mockDb.Setup(db => db.LoadData<DocumentColumn>(StoredProcedureDocmapper.AddNewDocmapperColumn, It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(new List<DocumentColumn>() { expectedColumn });

            // Act

            DocumentColumn result = await documentColumnSqlServerData.AddDocumentColumnAsync(columnToAdd);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(expectedColumn.DocmapperColumnId, result.DocmapperColumnId);
            Assert.Equal(expectedColumn.ElementName, result.ElementName);
            Assert.Equal(expectedColumn.SystemColumnName, result.SystemColumnName);
        }

        [Fact]
        public async Task AddDocumentColumnAsync_InvalidColumn_ThrowsException()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentColumnSqlServerData>> mockLogger = new();

            DocumentColumnSqlServerData documentColumnSqlServerData = new(mockDb.Object, mockLogger.Object);

            DocumentColumn columnToAdd = new() { ElementName = "NewColumn", SystemColumnName = "NewSystemColumn" };

            _ = mockDb.Setup(db => db.SaveData(StoredProcedureDocmapper.AddNewDocmapperColumn, It.IsAny<object>(), It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("Simulated exception"));

            // Act & Assert

            _ = await Assert.ThrowsAsync<InvalidOperationException>(() => documentColumnSqlServerData.AddDocumentColumnAsync(columnToAdd));
        }
    }
}