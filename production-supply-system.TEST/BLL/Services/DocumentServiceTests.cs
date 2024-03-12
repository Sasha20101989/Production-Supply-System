using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Services;

using DAL.Data.Repositories.Contracts;
using DAL.Models.Document;

using Moq;
using Xunit;

namespace production_supply_system.TEST.BLL.Services
{
    public class DocumentServiceTests
    {
        [Fact]
        public async Task CreateDocumentAsync_ShouldCreateDocumentAndContent()
        {
            // Arrange

            Docmapper document = new() { };

            List<DocmapperContent> documentContent = new() { };

            Mock<IRepository<Docmapper>> documentMapperRepositoryMock = new();

            Mock<IRepository<DocmapperColumn>> documentColumnRepositoryMock = new();

            Mock<IRepository<DocmapperContent>> documentContentRepositoryMock = new();


            DocumentService service = new(
                documentMapperRepositoryMock.Object,
                documentColumnRepositoryMock.Object,
                documentContentRepositoryMock.Object);

            _ = documentMapperRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Docmapper>(), It.IsAny<Enum>(), It.IsAny<object>()))
                .ReturnsAsync(new Docmapper { Id = 1 });

            _ = documentContentRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<DocmapperContent>(), It.IsAny<Enum>(), It.IsAny<object>()))
                .ReturnsAsync(new DocmapperContent { Id = 1 });

            // Act

            Docmapper result = await service.CreateDocumentAsync(document, documentContent);

            // Assert

            Assert.NotNull(result);

            Assert.NotNull(result.DocmapperContents);

            Assert.Equal(documentContent.Count, result.DocmapperContents.Count());

            documentMapperRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Docmapper>(), It.IsAny<Enum>(), It.IsAny<object>()), Times.Once);

            documentContentRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<DocmapperContent>(), It.IsAny<Enum>(), It.IsAny<object>()), Times.Exactly(documentContent.Count));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDocuments()
        {
            // Arrange

            Mock<IRepository<Docmapper>> documentMapperRepositoryMock = new();

            Mock<IRepository<DocmapperColumn>> documentColumnRepositoryMock = new();

            Mock<IRepository<DocmapperContent>> documentContentRepositoryMock = new();


            DocumentService service = new(
                documentMapperRepositoryMock.Object,
                documentColumnRepositoryMock.Object,
                documentContentRepositoryMock.Object);

            List<Docmapper> documents = new() { new Docmapper { Id = 1 }, new Docmapper { Id = 2 } };
            _ = documentMapperRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(documents);

            // Act

            IEnumerable<Docmapper> result = await service.GetAllAsync();

            // Assert

            Assert.NotNull(result);

            Assert.Equal(documents.Count, result.Count());

            documentMapperRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
        [Fact]

        public async Task GetDocumentByIdAsync_ShouldReturnDocumentById()

        {

            // Arrange

            var documentId = 1;

            Mock<IRepository<Docmapper>> documentMapperRepositoryMock = new();

            Mock<IRepository<DocmapperColumn>> documentColumnRepositoryMock = new();

            Mock<IRepository<DocmapperContent>> documentContentRepositoryMock = new();


            DocumentService service = new(
                documentMapperRepositoryMock.Object,
                documentColumnRepositoryMock.Object,
                documentContentRepositoryMock.Object);



            var document = new Docmapper { Id = documentId };

            documentMapperRepositoryMock.Setup(repo => repo.GetByIdAsync(documentId))

                .ReturnsAsync(document);



            // Act

            var result = await service.GetDocumentByIdAsync(documentId);



            // Assert

            Assert.NotNull(result);

            Assert.Equal(documentId, result.Id);

            documentMapperRepositoryMock.Verify(repo => repo.GetByIdAsync(documentId), Times.Once);

        }



        [Fact]

        public async Task GetAllColumnsAsync_ShouldReturnAllColumns()

        {

            // Arrange

            Mock<IRepository<Docmapper>> documentMapperRepositoryMock = new();

            Mock<IRepository<DocmapperColumn>> documentColumnRepositoryMock = new();

            Mock<IRepository<DocmapperContent>> documentContentRepositoryMock = new();


            DocumentService service = new(
                documentMapperRepositoryMock.Object,
                documentColumnRepositoryMock.Object,
                documentContentRepositoryMock.Object);



            var columns = new List<DocmapperColumn> { new DocmapperColumn { Id = 1 }, new DocmapperColumn { Id = 2 } };

            documentColumnRepositoryMock.Setup(repo => repo.GetAllAsync())

                .ReturnsAsync(columns);



            // Act

            var result = await service.GetAllColumnsAsync();



            // Assert

            Assert.NotNull(result);

            Assert.Equal(columns.Count, result.Count());

            documentColumnRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);

        }



        [Fact]

        public async Task UpdateDocumentAsync_ShouldUpdateDocumentAndContent()

        {

            // Arrange

            var document = new Docmapper { /* initialize properties */ };

            var documentContent = new List<DocmapperContent> { /* initialize content items */ };

            Mock<IRepository<Docmapper>> documentMapperRepositoryMock = new();

            Mock<IRepository<DocmapperColumn>> documentColumnRepositoryMock = new();

            Mock<IRepository<DocmapperContent>> documentContentRepositoryMock = new();


            DocumentService service = new(
                documentMapperRepositoryMock.Object,
                documentColumnRepositoryMock.Object,
                documentContentRepositoryMock.Object);



            documentMapperRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Docmapper>(), It.IsAny<Enum>(), It.IsAny<object>()))

                .Returns(Task.CompletedTask);



            documentContentRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<DocmapperContent>()))

                .ReturnsAsync(true);



            documentContentRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<DocmapperContent>(), It.IsAny<Enum>(), It.IsAny<object>()))

                .Returns(Task.CompletedTask);



            documentContentRepositoryMock.Setup(repo => repo.GetAllAsync())

                .ReturnsAsync(new List<DocmapperContent>());



            // Act

            await service.UpdateDocumentAsync(document, documentContent);



            // Assert

            documentMapperRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Docmapper>(), It.IsAny<Enum>(), It.IsAny<object>()), Times.Once);

            documentContentRepositoryMock.Verify(repo => repo.ExistsAsync(It.IsAny<DocmapperContent>()), Times.Exactly(documentContent.Count));

            documentContentRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<DocmapperContent>(), It.IsAny<Enum>(), It.IsAny<object>()), Times.Exactly(documentContent.Count));

        }



        [Fact]

        public async Task AddDocumentColumnAsync_ShouldAddDocumentColumn()

        {

            // Arrange

            var documentColumn = new DocmapperColumn { /* initialize properties */ };



            Mock<IRepository<Docmapper>> documentMapperRepositoryMock = new();

            Mock<IRepository<DocmapperColumn>> documentColumnRepositoryMock = new();

            Mock<IRepository<DocmapperContent>> documentContentRepositoryMock = new();


            DocumentService service = new(
                documentMapperRepositoryMock.Object,
                documentColumnRepositoryMock.Object,
                documentContentRepositoryMock.Object);



            documentColumnRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<DocmapperColumn>(), It.IsAny<Enum>(), It.IsAny<object>()))

                .ReturnsAsync(new DocmapperColumn { Id = 1 });



            // Act

            var result = await service.AddDocumentColumnAsync(documentColumn);



            // Assert

            Assert.NotNull(result);

            documentColumnRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<DocmapperColumn>(), It.IsAny<Enum>(), It.IsAny<object>()), Times.Once);

        }



        [Fact]

        public async Task GetAllDocumentContentItemsByIdAsync_ShouldReturnAllContentItemsForDocument()

        {

            // Arrange

            var documentId = 1;

            Mock<IRepository<Docmapper>> documentMapperRepositoryMock = new();

            Mock<IRepository<DocmapperColumn>> documentColumnRepositoryMock = new();

            Mock<IRepository<DocmapperContent>> documentContentRepositoryMock = new();


            DocumentService service = new(
                documentMapperRepositoryMock.Object,
                documentColumnRepositoryMock.Object,
                documentContentRepositoryMock.Object);



            var document = new Docmapper { Id = documentId };

            var contentItems = new List<DocmapperContent> { new DocmapperContent { Id = 1, DocmapperId = documentId }, new DocmapperContent { Id = 2, DocmapperId = documentId } };



            documentMapperRepositoryMock.Setup(repo => repo.GetByIdAsync(documentId))

                .ReturnsAsync(document);



            documentContentRepositoryMock.Setup(repo => repo.GetAllAsync())

                .ReturnsAsync(contentItems);



            documentColumnRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))

                .ReturnsAsync(new DocmapperColumn { Id = 1 });



            // Act

            IEnumerable<DocmapperContent> result = await service.GetAllDocumentContentItemsByIdAsync(documentId);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(contentItems.Count, result.Count());

            documentMapperRepositoryMock.Verify(repo => repo.GetByIdAsync(documentId), Times.Once);

            documentContentRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);

            documentColumnRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Exactly(contentItems.Count));
        }
    }
}
