using System.Collections.Generic;
using System.Threading.Tasks;

using BLL.Contracts;

using DAL.Data.Repositories.Contracts;
using DAL.Models.Docmapper;

using Document = DAL.Models.Docmapper.Document;

namespace BLL.Services
{
    /// <summary>
    /// Интерфейс сервиса для операций, связанных с картами сопоставления данных для эксель.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentMapperRepository _documentMapperRepository;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DocumentService"/>.
        /// </summary>
        /// <param name="documentMapperRepository">Репозиторий для доступа к информации о картах сопоставления данных для эксель.</param>
        public DocumentService(IDocumentMapperRepository documentMapperRepository)
        {
            _documentMapperRepository = documentMapperRepository;
        }

        /// <inheritdoc />
        public async Task<Document> CreateDocumentAsync(Document document, List<DocumentContent> documentContent)
        {
            return await _documentMapperRepository.CreateDocumentAsync(document, documentContent);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
        {
            return await _documentMapperRepository.GetAllDocumentsAsync();
        }

        /// <inheritdoc />
        public async Task<Document> GetDocumentByIdAsync(int mapId)
        {
            return await _documentMapperRepository.GetDocumentByIdAsync(mapId);
        }

        /// <inheritdoc />
        public async Task CreateDocumentContentAsync(DocumentContent content)
        {
            await _documentMapperRepository.CreateDocumentContentAsync(content);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DocumentColumn>> GetAllColumnsAsync()
        {
            return await _documentMapperRepository.GetAllColumnsAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DocumentContent>> GetAllDocumentContentItemsByIdAsync(int mapId)
        {
            return await _documentMapperRepository.GetAllDocumentContentItemsByIdAsync(mapId);
        }

        /// <inheritdoc />
        public async Task UpdateDocumentAsync(Document document, List<DocumentContent> documentContent)
        {
            await _documentMapperRepository.UpdateDocumentAsync(document, documentContent);
        }

        /// <inheritdoc />
        public async Task<DocumentColumn> AddDocumentColumnAsync(DocumentColumn documentColumn)
        {
            return await _documentMapperRepository.AddDocumentColumnAsync(documentColumn);
        }
    }
}
