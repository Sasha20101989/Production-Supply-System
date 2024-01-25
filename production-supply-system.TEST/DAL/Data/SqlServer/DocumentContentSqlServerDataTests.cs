using DAL.Data.SqlServer;
using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models.Docmapper;
using Microsoft.Extensions.Logging;
using Moq;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace production_supply_system.TEST.DAL.Data.SqlServer
{
    public class DocumentContentSqlServerDataTests
    {
        [Fact]
        public async Task CreateDocumentContentAsync_ValidContent_ReturnsCreatedContent()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentColumnSqlServerData>> mockLogger = new();

            DocumentContentSqlServerData documentContentSqlServerData = new(mockDb.Object, mockLogger.Object);

            DocumentContent contentToCreate = new() { DocmapperId = 1, DocmapperColumnId = 1, RowNumber = 1, ColumnNumber = 1 };

            List<DocumentContent> expectedContent = new()
            {
                new DocumentContent { DocmapperContentId = 1, DocmapperId = 1, DocmapperColumnId = 1, RowNumber = 1, ColumnNumber = 1 }
            };

            _ = mockDb.Setup(db => db.LoadData<DocumentContent>(StoredProcedureDocmapper.AddNewDocmapperContent, It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(expectedContent);

            // Act

            await documentContentSqlServerData.CreateDocumentContentAsync(contentToCreate);

            // Assert

            Assert.Equal(1, contentToCreate.DocmapperContentId);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingContentId_ReturnsContent()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentColumnSqlServerData>> mockLogger = new();

            DocumentContentSqlServerData documentContentSqlServerData = new(mockDb.Object, mockLogger.Object);

            int existingContentId = 1;

            List<DocumentContent> expectedContent = new()
            {
                new DocumentContent { DocmapperContentId = 1, DocmapperId = 1, DocmapperColumnId = 1, RowNumber = 1, ColumnNumber = 1 }
            };

            _ = mockDb.Setup(db => db.LoadData<DocumentContent>(StoredProcedureDocmapper.GetAllDocmapperContentItems, It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(expectedContent);

            // Act

            DocumentContent result = await documentContentSqlServerData.GetByIdAsync(existingContentId);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(existingContentId, result.DocmapperContentId);

        }

        [Fact]
        public async Task ExistsAsync_ExistingContent_ReturnsTrue()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentColumnSqlServerData>> mockLogger = new();

            DocumentContentSqlServerData documentContentSqlServerData = new(mockDb.Object, mockLogger.Object);

            int existingContentId = 1;

            List<DocumentContent> expectedContent = new()
            {
                new DocumentContent { DocmapperContentId = 1, DocmapperId = 1, DocmapperColumnId = 1, RowNumber = 1, ColumnNumber = 1 }
            };

            _ = mockDb.Setup(db => db.LoadData<DocumentContent>(StoredProcedureDocmapper.GetAllDocmapperContentItems, It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(expectedContent);

            // Act

            bool exists = await documentContentSqlServerData.ExistsAsync(new DocumentContent { DocmapperContentId = existingContentId });

            // Assert

            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_NonExistingContent_ReturnsFalse()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentColumnSqlServerData>> mockLogger = new();

            DocumentContentSqlServerData documentContentSqlServerData = new(mockDb.Object, mockLogger.Object);

            int nonExistingContentId = 99;

            List<DocumentContent> expectedContent = new()
            {
                new DocumentContent { DocmapperContentId = 1, DocmapperId = 1, DocmapperColumnId = 1, RowNumber = 1, ColumnNumber = 1 }
            };

            _ = mockDb.Setup(db => db.LoadData<DocumentContent>(StoredProcedureDocmapper.GetAllDocmapperContentItems, It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(expectedContent);

            // Act

            bool exists = await documentContentSqlServerData.ExistsAsync(new DocumentContent { DocmapperContentId = nonExistingContentId });

            // Assert

            Assert.False(exists);
        }

        [Fact]
        public async Task DeleteAsync_ExistingContentId_DeletesContent()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentColumnSqlServerData>> mockLogger = new();

            DocumentContentSqlServerData documentContentSqlServerData = new(mockDb.Object, mockLogger.Object);

            int existingContentId = 1;

            List<DocumentContent> expectedContent = new()
            {
                new DocumentContent { DocmapperContentId = 1, DocmapperId = 1, DocmapperColumnId = 1, RowNumber = 1, ColumnNumber = 1 }
            };

            _ = mockDb.Setup(db => db.LoadData<DocumentContent>(StoredProcedureDocmapper.GetAllDocmapperContentItems, It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(expectedContent);

            // Act

            await documentContentSqlServerData.DeleteAsync(existingContentId);

            // Assert

            mockDb.Verify(db => db.SaveData(StoredProcedureDocmapper.DeleteDocmapperContent, It.IsAny<object>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ExistingContent_ReturnsNoException()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentColumnSqlServerData>> mockLogger = new();

            DocumentContentSqlServerData documentContentSqlServerData = new(mockDb.Object, mockLogger.Object);

            DocumentContent existingContent = new() { DocmapperContentId = 1, RowNumber = 2, ColumnNumber = 2 };

            _ = mockDb.Setup(db => db.SaveData(StoredProcedureDocmapper.UpdateDocmapperContent, It.IsAny<object>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert

            _ = await Assert.ThrowsAsync<Exception>(() => documentContentSqlServerData.UpdateAsync(existingContent));

        }
    }
}
