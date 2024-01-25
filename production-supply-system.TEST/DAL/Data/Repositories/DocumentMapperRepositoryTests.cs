using DAL.Data.Contracts;
using DAL.Data.Repositories;
using DAL.Models.Docmapper;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace production_supply_system.TEST.DAL.Data.Repositories
{
    public class DocumentMapperRepositoryTests
    {
        [Fact]
        public async Task GetAllDocumentsAsync_ReturnsAllDocuments()
        {
            // Arrange

            Mock<IDocumentData> documentDataMock = new();

            _ = documentDataMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Document>());

            DocumentMapperRepository repository = new(documentDataMock.Object, null, null);

            // Act

            IEnumerable<Document> result = await repository.GetAllDocumentsAsync();

            // Assert

            Assert.NotNull(result);

            _ = Assert.IsType<List<Document>>(result);
        }

        [Fact]
        public async Task CreateDocumentAsync_CreatesDocumentWithContent()
        {
            // Arrange

            Mock<IDocumentData> documentDataMock = new();

            _ = documentDataMock.Setup(repo => repo.CreateDocumentAsync(It.IsAny<Document>())).ReturnsAsync(new Document { DocmapperId = 1 });

            Mock<IDocumentContentData> documentContentDataMock = new();

            _ = documentContentDataMock.Setup(repo => repo.CreateDocumentContentAsync(It.IsAny<DocumentContent>())).Returns(Task.CompletedTask);

            DocumentMapperRepository repository = new(
                documentDataMock.Object,
                documentContentDataMock.Object,
                Mock.Of<IDocumentColumnData>()
            );

            Document document = new();

            List<DocumentContent> documentContent = new() { new DocumentContent(), new DocumentContent() };

            // Act

            Document result = await repository.CreateDocumentAsync(document, documentContent);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(1, result.DocmapperId);

            documentDataMock.Verify(
                repo => repo.CreateDocumentAsync(It.IsAny<Document>()),
                Times.Once
            );

            documentContentDataMock.Verify(
                repo => repo.CreateDocumentContentAsync(It.IsAny<DocumentContent>()),
                Times.Exactly(documentContent.Count)
            );

            foreach (DocumentContent content in documentContent)
            {
                Assert.Equal(1, content.DocmapperId);
            }
        }

        [Fact]
        public async Task GetDocumentByIdAsync_ReturnsDocument()
        {
            // Arrange

            Mock<IDocumentData> documentDataMock = new();

            _ = documentDataMock.Setup(repo => repo.GetDocumentByIdAsync(It.IsAny<int>())).ReturnsAsync(new Document());

            DocumentMapperRepository repository = new(documentDataMock.Object, null, null);

            // Act

            Document result = await repository.GetDocumentByIdAsync(1);

            // Assert

            Assert.NotNull(result);

            _ = Assert.IsType<Document>(result);

            documentDataMock.Verify(repo => repo.GetDocumentByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDocumentAsync_UpdatesDocumentAndContent()
        {
            // Arrange

            Mock<IDocumentData> documentDataMock = new();

            _ = documentDataMock.Setup(repo => repo.UpdateDocumentAsync(It.IsAny<Document>())).Returns(Task.CompletedTask);

            Mock<IDocumentColumnData> documentColumnDataMock = new();

            Mock<IDocumentContentData> documentContentDataMock = new();

            _ = documentContentDataMock.Setup(repo => repo.UpdateAsync(It.IsAny<DocumentContent>())).Returns(Task.CompletedTask);

            _ = documentContentDataMock.Setup(repo => repo.GetAllDocumentContentItemsByIdAsync(It.IsAny<int>())).ReturnsAsync(new List<DocumentContent>());

            _ = documentContentDataMock.Setup(repo => repo.Refresh());

            DocumentMapperRepository repository = new(documentDataMock.Object, documentContentDataMock.Object, documentColumnDataMock.Object);

            // Act

            await repository.UpdateDocumentAsync(new Document(), new List<DocumentContent>());

            // Assert

            documentDataMock.Verify(repo => repo.UpdateDocumentAsync(It.IsAny<Document>()), Times.Once);

            documentContentDataMock.Verify(repo => repo.UpdateAsync(It.IsAny<DocumentContent>()), Times.Exactly(0)); // No update in this case

            documentContentDataMock.Verify(repo => repo.Refresh(), Times.Once);

        }

        [Fact]
        public async Task CreateDocumentContentAsync_CreatesDocumentContent()
        {
            // Arrange

            Mock<IDocumentContentData> documentContentDataMock = new();

            DocumentMapperRepository repository = new(

                Mock.Of<IDocumentData>(),

                documentContentDataMock.Object,

                Mock.Of<IDocumentColumnData>()
            );

            DocumentContent content = new();

            // Act

            await repository.CreateDocumentContentAsync(content);

            // Assert

            documentContentDataMock.Verify(
                repo => repo.CreateDocumentContentAsync(It.IsAny<DocumentContent>()),
                Times.Once
            );
        }

        [Fact]
        public async Task GetAllDocumentContentItemsByIdAsync_ReturnsDocumentContentsWithDetails()
        {
            // Arrange

            Mock<IDocumentData> documentDataMock = new();

            _ = documentDataMock.Setup(repo => repo.GetDocumentByIdAsync(It.IsAny<int>())).ReturnsAsync(new Document());

            Mock<IDocumentContentData> documentContentDataMock = new();

            _ = documentContentDataMock.Setup(repo => repo.GetAllDocumentContentItemsByIdAsync(It.IsAny<int>())).ReturnsAsync(
                new List<DocumentContent> { new DocumentContent { DocmapperColumnId = 1 } }
            );

            Mock<IDocumentColumnData> documentColumnDataMock = new();

            _ = documentColumnDataMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new DocumentColumn());

            DocumentMapperRepository repository = new(
                documentDataMock.Object,
                documentContentDataMock.Object,
                documentColumnDataMock.Object
            );

            // Act

            IEnumerable<DocumentContent> result = await repository.GetAllDocumentContentItemsByIdAsync(1);

            // Assert

            Assert.NotNull(result);

            _ = Assert.Single(result);

            documentDataMock.Verify(
                repo => repo.GetDocumentByIdAsync(It.IsAny<int>()),
                Times.Once
            );

            documentContentDataMock.Verify(
                repo => repo.GetAllDocumentContentItemsByIdAsync(It.IsAny<int>()),
                Times.Once
            );

            documentColumnDataMock.Verify(
                repo => repo.GetByIdAsync(It.IsAny<int>()),
                Times.Once
            );
        }

        [Fact]
        public async Task GetAllColumnsAsync_ReturnsAllColumns()
        {
            // Arrange

            Mock<IDocumentColumnData> documentColumnDataMock = new();

            List<DocumentColumn> expectedColumns = new()
            {
                new DocumentColumn { DocmapperColumnId = 1, ElementName = "Column1" },
                new DocumentColumn { DocmapperColumnId = 2, ElementName = "Column2" },
            };

            _ = documentColumnDataMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedColumns);



            DocumentMapperRepository repository = new(
                Mock.Of<IDocumentData>(),
                Mock.Of<IDocumentContentData>(),
                documentColumnDataMock.Object
            );

            // Act

            IEnumerable<DocumentColumn> result = await repository.GetAllColumnsAsync();

            // Assert

            Assert.NotNull(result);

            Assert.Equal(expectedColumns, result);

            documentColumnDataMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task AddDocumentColumnAsync_AddsColumn()
        {
            // Arrange

            Mock<IDocumentColumnData> documentColumnDataMock = new();

            _ = documentColumnDataMock.Setup(repo => repo.AddDocumentColumnAsync(It.IsAny<DocumentColumn>())).ReturnsAsync(new DocumentColumn { DocmapperColumnId = 1 });

            DocumentMapperRepository repository = new(Mock.Of<IDocumentData>(), Mock.Of<IDocumentContentData>(), documentColumnDataMock.Object);

            DocumentColumn documentColumn = new() { ElementName = "NewColumn" };

            // Act

            DocumentColumn result = await repository.AddDocumentColumnAsync(documentColumn);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(1, result.DocmapperColumnId);

            documentColumnDataMock.Verify(repo => repo.AddDocumentColumnAsync(It.IsAny<DocumentColumn>()), Times.Once);
        }
    }
}
