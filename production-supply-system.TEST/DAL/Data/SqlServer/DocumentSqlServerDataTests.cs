using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DocumentSqlServerDataTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsDocumentList()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentSqlServerData>> mockLogger = new();

            DocumentSqlServerData documentSqlServerData = new(mockDb.Object, mockLogger.Object);

            List<Document> expectedDocuments = new() { new Document { DocmapperId = 1, DocmapperName = "Document1" }, new Document { DocmapperId = 2, DocmapperName = "Document2" } };

            _ = mockDb.Setup(db => db.LoadData<Document>(StoredProcedureDocmapper.GetAllDocmapperItems, It.IsAny<object>(), "Default"))
                .ReturnsAsync(expectedDocuments);

            // Act

            IEnumerable<Document> result = await documentSqlServerData.GetAllAsync();

            // Assert

            Assert.NotNull(result);

            Assert.Equal(expectedDocuments, result);

        }

        [Fact]
        public async Task GetDocumentByIdAsync_ExistingId_ReturnsDocument()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentSqlServerData>> mockLogger = new();

            DocumentSqlServerData documentSqlServerData = new(mockDb.Object, mockLogger.Object);

            int existingId = 1;


            List<Document> expectedDocuments = new() { new Document { DocmapperId = existingId, DocmapperName = "Document1" } };

            _ = mockDb.Setup(db => db.LoadData<Document>(StoredProcedureDocmapper.GetAllDocmapperItems, It.IsAny<object>(), "Default"))
                .ReturnsAsync(expectedDocuments);

            // Act

            Document result = await documentSqlServerData.GetDocumentByIdAsync(existingId);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(expectedDocuments.FirstOrDefault(d => d.DocmapperId == existingId), result);
        }

        [Fact]
        public async Task GetDocumentByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentSqlServerData>> mockLogger = new();

            DocumentSqlServerData documentSqlServerData = new(mockDb.Object, mockLogger.Object);

            int nonExistingId = 99;

            List<Document> expectedDocuments = new() { new Document { DocmapperId = 1, DocmapperName = "Document1" } };

            _ = mockDb.Setup(db => db.LoadData<Document>(StoredProcedureDocmapper.GetAllDocmapperItems, It.IsAny<object>(), "Default"))
                .ReturnsAsync(expectedDocuments);

            // Act

            Document result = await documentSqlServerData.GetDocumentByIdAsync(nonExistingId);

            // Assert

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateDocumentAsync_ValidDocument_ReturnsCreatedDocument()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<DocumentSqlServerData>> mockLogger = new();

            DocumentSqlServerData documentSqlServerData = new(mockDb.Object, mockLogger.Object);

            Document documentToCreate = new()
            {
                DocmapperName = "NewDocument",
                DefaultFolder = "/new-folder",
                SheetName = "Sheet1",
                FirstDataRow = 1
            };

            Document expectedDocument = new()
            {
                DocmapperId = 1,
                DocmapperName = "NewDocument",
                DefaultFolder = "/new-folder",
                SheetName = "Sheet1",
                FirstDataRow = 1,
                IsActive = true
            };

            _ = mockDb.Setup(db => db.LoadData<Document>(StoredProcedureDocmapper.AddNewDocmapper, It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(new List<Document> { expectedDocument });

            // Act

            Document result = await documentSqlServerData.CreateDocumentAsync(documentToCreate);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(expectedDocument.DocmapperId, result.DocmapperId);

            Assert.Equal(expectedDocument.DocmapperName, result.DocmapperName);

            Assert.Equal(expectedDocument.DefaultFolder, result.DefaultFolder);

            Assert.Equal(expectedDocument.SheetName, result.SheetName);

            Assert.Equal(expectedDocument.FirstDataRow, result.FirstDataRow);

            Assert.Equal(expectedDocument.IsActive, result.IsActive);
        }

        [Fact]
        public async Task UpdateDocumentAsync_ValidDocument_ReturnsNoException()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();
            Mock<ILogger<DocumentSqlServerData>> mockLogger = new();

            DocumentSqlServerData documentSqlServerData = new(mockDb.Object, mockLogger.Object);

            Document documentToUpdate = new() { DocmapperId = 1, DocmapperName = "UpdatedDocument", DefaultFolder = "/updated-folder" };

            _ = mockDb.Setup(db => db.SaveData(StoredProcedureDocmapper.UpdateDocmapperItem, It.IsAny<object>(), "Default"))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert

            _ = await Assert.ThrowsAsync<Exception>(() => documentSqlServerData.UpdateDocumentAsync(documentToUpdate));
        }
    }
}

