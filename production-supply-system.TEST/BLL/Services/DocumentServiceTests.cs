using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Services;
using DAL.Data.Repositories.Contracts;
using DAL.Models.Docmapper;
using Moq;
using Xunit;

namespace production_supply_system.TEST.BLL.Services
{
    public class DocumentServiceTests
    {
        [Fact]
        public async Task CreateDocumentAsync_CallsRepository()
        {
            // Arrange

            Mock<IDocumentMapperRepository> documentMapperRepositoryMock = new();
            DocumentService service = new(documentMapperRepositoryMock.Object);
            Document document = new();

            List<DocumentContent> documentContent = new();

            // Act

            _ = await service.CreateDocumentAsync(document, documentContent);

            // Assert

            documentMapperRepositoryMock.Verify(repo => repo.CreateDocumentAsync(document, documentContent), Times.Once);
        }

        [Fact]
        public async Task GetAllDocumentsAsync_CallsRepository()
        {
            // Arrange

            Mock<IDocumentMapperRepository> documentMapperRepositoryMock = new();
            DocumentService service = new(documentMapperRepositoryMock.Object);

            // Act

            _ = await service.GetAllDocumentsAsync();

            // Assert

            documentMapperRepositoryMock.Verify(repo => repo.GetAllDocumentsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetDocumentByIdAsync_CallsRepository()
        {
            // Arrange

            Mock<IDocumentMapperRepository> documentMapperRepositoryMock = new();
            DocumentService service = new(documentMapperRepositoryMock.Object);

            int mapId = 1;

            // Act

            _ = await service.GetDocumentByIdAsync(mapId);

            // Assert

            documentMapperRepositoryMock.Verify(repo => repo.GetDocumentByIdAsync(mapId), Times.Once);
        }

        [Fact]
        public async Task CreateDocumentContentAsync_CallsRepository()
        {
            // Arrange

            Mock<IDocumentMapperRepository> documentMapperRepositoryMock = new();
            DocumentService service = new(documentMapperRepositoryMock.Object);
            DocumentContent content = new();

            // Act

            await service.CreateDocumentContentAsync(content);

            // Assert

            documentMapperRepositoryMock.Verify(repo => repo.CreateDocumentContentAsync(content), Times.Once);
        }

        [Fact]

        public async Task GetAllColumnsAsync_CallsRepository()
        {

            // Arrange

            Mock<IDocumentMapperRepository> documentMapperRepositoryMock = new();

            DocumentService service = new(documentMapperRepositoryMock.Object);

            // Act

            _ = await service.GetAllColumnsAsync();

            // Assert

            documentMapperRepositoryMock.Verify(repo => repo.GetAllColumnsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllDocumentContentItemsByIdAsync_CallsRepository()
        {
            // Arrange

            Mock<IDocumentMapperRepository> documentMapperRepositoryMock = new();

            DocumentService service = new(documentMapperRepositoryMock.Object);

            int mapId = 1;

            // Act

            _ = await service.GetAllDocumentContentItemsByIdAsync(mapId);

            // Assert

            documentMapperRepositoryMock.Verify(repo => repo.GetAllDocumentContentItemsByIdAsync(mapId), Times.Once);
        }

        [Fact]
        public async Task UpdateDocumentAsync_CallsRepository()
        {
            // Arrange

            Mock<IDocumentMapperRepository> documentMapperRepositoryMock = new();

            DocumentService service = new(documentMapperRepositoryMock.Object);

            Document document = new();

            List<DocumentContent> documentContent = new();

            // Act

            await service.UpdateDocumentAsync(document, documentContent);

            // Assert

            documentMapperRepositoryMock.Verify(repo => repo.UpdateDocumentAsync(document, documentContent), Times.Once);
        }

        [Fact]
        public async Task AddDocumentColumnAsync_CallsRepository()
        {
            // Arrange

            Mock<IDocumentMapperRepository> documentMapperRepositoryMock = new();

            DocumentService service = new(documentMapperRepositoryMock.Object);

            DocumentColumn documentColumn = new();

            // Act

            _ = await service.AddDocumentColumnAsync(documentColumn);

            // Assert

            documentMapperRepositoryMock.Verify(repo => repo.AddDocumentColumnAsync(documentColumn), Times.Once);
        }
    }
}
