using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data.Contracts;
using DAL.Data.Repositories.Contracts;
using DAL.Models.Docmapper;

namespace DAL.Data.Repositories
{
    /// <summary>
    /// Реализация репозитория для взаимодействия с картами сопоставления данных в эксель.
    /// </summary>
    public class DocumentMapperRepository : IDocumentMapperRepository
    {
        private readonly IDocumentData _documentData;
        private readonly IDocumentColumnData _documentColumnData;
        private readonly IDocumentContentData _documentContentData;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="DocumentMapperRepository"/> class.
        /// </summary>
        /// <param name="documentData">Источник данных для информации о картах</param>
        public DocumentMapperRepository(IDocumentData documentData, IDocumentContentData documentContentData, IDocumentColumnData documentColumnData)
        {
            _documentData = documentData;
            _documentContentData = documentContentData;
            _documentColumnData = documentColumnData;
        }

        #region Document

        /// <inheritdoc />
        public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
        {
            return await _documentData.GetAllAsync();
        }

        /// <inheritdoc />
        public async Task<Document> CreateDocumentAsync(Document document, List<DocumentContent> documentContent)
        {
            Document result = await _documentData.CreateDocumentAsync(document); ;

            foreach (DocumentContent content in documentContent)
            {
                content.DocmapperId = result.DocmapperId;
                await CreateDocumentContentAsync(content);
            }

            _documentData.Refresh();
            _documentContentData.Refresh();

            return result;
        }

        /// <inheritdoc />
        public async Task<Document> GetDocumentByIdAsync(int mapId)
        {
            return await _documentData.GetDocumentByIdAsync(mapId);
        }

        /// <inheritdoc />
        public async Task UpdateDocumentAsync(Document document, List<DocumentContent> documentContent)
        {
            await _documentData.UpdateDocumentAsync(document);

            await ProcessDocumentContentAsync(document.DocmapperId, documentContent);

            _documentData.Refresh();
        }

        #endregion Document

        #region DocumentContent

        private async Task ProcessDocumentContentAsync(int mapId, List<DocumentContent> documentContent)
        {
            IEnumerable<DocumentContent> cachedContentItems = await GetAllDocumentContentItemsByIdAsync(mapId);

            await DeleteExistingItemsAsync(documentContent, cachedContentItems);

            await UpdateDocumentContent(documentContent);

            _documentContentData.Refresh();
        }

        private async Task DeleteExistingItemsAsync(List<DocumentContent> documentContent, IEnumerable<DocumentContent> cachedContentItems)
        {
            IEnumerable<DocumentContent> missingContent = cachedContentItems
                .Where(ca => !documentContent.Any(dc => dc.DocmapperContentId == ca.DocmapperContentId));

            foreach (DocumentContent item in missingContent)
            {
                await _documentContentData.DeleteAsync(item.DocmapperContentId);
            }
        }

        private async Task UpdateDocumentContent(List<DocumentContent> documentContent)
        {
            foreach (DocumentContent content in documentContent)
            {
                if (!await _documentContentData.ExistsAsync(content))
                {
                    await CreateDocumentContentAsync(content);
                }
                else
                {
                    await _documentContentData.UpdateAsync(content);
                }
            }
        }

        /// <inheritdoc />
        public async Task CreateDocumentContentAsync(DocumentContent content)
        {
            await _documentContentData.CreateDocumentContentAsync(content);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DocumentContent>> GetAllDocumentContentItemsByIdAsync(int mapId)
        {
            Document document = await _documentData.GetDocumentByIdAsync(mapId);

            IEnumerable<DocumentContent> content = await _documentContentData.GetAllDocumentContentItemsByIdAsync(mapId);

            foreach (DocumentContent item in content)
            {
                item.Document = document;
                item.DocumentColumn = await _documentColumnData.GetByIdAsync(item.DocmapperColumnId);
            }

            return content;
        }

        #endregion DocumentContent

        #region DocumentColumn

        /// <inheritdoc />
        public async Task<IEnumerable<DocumentColumn>> GetAllColumnsAsync()
        {
            return await _documentColumnData.GetAllAsync();
        }

        /// <inheritdoc />
        public async Task<DocumentColumn> AddDocumentColumnAsync(DocumentColumn documentColumn)
        {
            return await _documentColumnData.AddDocumentColumnAsync(documentColumn);
        }

        #endregion DocumentColumn
    }
}
