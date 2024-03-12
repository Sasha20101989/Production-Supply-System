using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Contracts;
using DAL.Data.Repositories.Contracts;
using DAL.Enums;
using DAL.Models.Document;
using DAL.Parameters.Document;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BLL.Services
{
    /// <summary>
    /// Интерфейс сервиса для операций, связанных с картами сопоставления данных для эксель.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly IRepository<Docmapper> _documentMapperRepository;
        private readonly IRepository<DocmapperColumn> _documentColumnRepository;
        private readonly IRepository<DocmapperContent> _documentContentRepository;
        private readonly ILogger<DocumentService> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DocumentService"/>.
        /// </summary>
        /// <param name="documentMapperRepository">Репозиторий для доступа к информации о картах сопоставления данных для эксель.</param>
        /// <param name="logger">Регистратор для отслеживания информации и ошибок.</param>
        public DocumentService(
            IRepository<Docmapper> documentMapperRepository, 
            IRepository<DocmapperColumn> documentColumnRepository, 
            IRepository<DocmapperContent> documentContentRepository,
            ILogger<DocumentService> logger)
        {
            _documentMapperRepository = documentMapperRepository;
            _documentColumnRepository = documentColumnRepository;
            _documentContentRepository = documentContentRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<Docmapper> CreateDocumentAsync(Docmapper document, List<DocmapperContent> documentContent)
        {
            CreateDocmapperParameters parameters = new(document);

            try
            {
                _logger.LogInformation($"Start saving a document: '{JsonConvert.SerializeObject(document)}'");

                Docmapper newDocument = await _documentMapperRepository.CreateAsync(document, StoredProcedureDocmapper.AddNewDocmapper, parameters);

                foreach (DocmapperContent contentItem in documentContent)
                {
                    if (newDocument.Id <= 0)
                    {
                        throw new Exception("DocmapperId should be greater than 0.");
                    }

                    contentItem.DocmapperId = newDocument.Id;

                    contentItem.Docmapper = newDocument;

                    await CreateDocumentContentAsync(contentItem);
                }

                newDocument.DocmapperContents = documentContent;

                try
                {
                    _documentMapperRepository.RefreshData();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error refresh documents: {JsonConvert.SerializeObject(ex)}");

                    throw;
                }

                return newDocument;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new document: {JsonConvert.SerializeObject(ex)}");

                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Docmapper>> GetAllAsync()
        {
            try
            {
                return await _documentMapperRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get documents list: {JsonConvert.SerializeObject(ex)}");

                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Docmapper> GetDocumentByIdAsync(int mapId)
        {
            try
            {
                return await _documentMapperRepository.GetByIdAsync(mapId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get document by id {mapId}: {JsonConvert.SerializeObject(ex)}");

                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DocmapperColumn>> GetAllColumnsAsync()
        {
            try
            {
                return await _documentColumnRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get document columns list: {JsonConvert.SerializeObject(ex)}");

                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateDocumentAsync(Docmapper document, List<DocmapperContent> documentContent)
        {
            UpdateDocmapperParameters parameters = new(document);

            try
            {
                await _documentMapperRepository.UpdateAsync(document, StoredProcedureDocmapper.UpdateDocmapperItem, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error update document: {JsonConvert.SerializeObject(document)}: {JsonConvert.SerializeObject(ex)}");

                throw;
            }

            IEnumerable<DocmapperContent> cachedContentItems = await GetAllDocumentContentItemsByIdAsync(document.Id);

            await DeleteExistingItemsAsync(documentContent, cachedContentItems);

            await UpdateDocumentContentAsync(documentContent);

            try
            {
                _documentMapperRepository.RefreshData();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error refresh documents: {JsonConvert.SerializeObject(ex)}");

                throw;
            }

            try
            {
                _documentContentRepository.RefreshData();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error refresh document content: {JsonConvert.SerializeObject(ex)}");

                throw;
            }
        }

        /// <inheritdoc />
        public async Task<DocmapperColumn> AddDocumentColumnAsync(DocmapperColumn documentColumn)
        {
            CreateDocmapperColumnParameters parameters = new(documentColumn);

            DocmapperColumn column;

            try
            {
                column = await _documentColumnRepository.CreateAsync(documentColumn, StoredProcedureDocmapper.AddNewDocmapperColumn, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new document column {JsonConvert.SerializeObject(documentColumn)}: {JsonConvert.SerializeObject(ex)}");

                throw;
            }

            try
            {
                _documentColumnRepository.RefreshData();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error refresh document columns: {JsonConvert.SerializeObject(ex)}");

                throw;
            }

            return column;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DocmapperContent>> GetAllDocumentContentItemsByIdAsync(int mapId)
        {
            //Docmapper document;

            //try
            //{
            //    document = await _documentMapperRepository.GetByIdAsync(mapId);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"Error get document by id {mapId}: {JsonConvert.SerializeObject(ex)}");

            //    throw;
            //}

            IEnumerable<DocmapperContent> documentContent;

            try
            {
                documentContent = (await _documentContentRepository.GetAllAsync())
                                                                        .Where(con => con.DocmapperId == mapId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get list document contents for document with id {mapId}: {JsonConvert.SerializeObject(ex)}");

                throw;
            }

            foreach (DocmapperContent item in documentContent)
            {
                try
                {
                    item.DocmapperColumn = await _documentColumnRepository.GetByIdAsync(item.DocmapperColumnId);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error get document column by id {item.DocmapperColumnId}: {JsonConvert.SerializeObject(ex)}");

                    throw;
                }
            }

            return documentContent;
        }

        private async Task DeleteExistingItemsAsync(List<DocmapperContent> documentContent, IEnumerable<DocmapperContent> cachedContentItems)
        {
            IEnumerable<DocmapperContent> missingContent = cachedContentItems
                .Where(ca => !documentContent.Any(dc => dc.Id == ca.Id));

            foreach (DocmapperContent item in missingContent)
            {
                try
                {
                    await _documentContentRepository.RemoveAsync(item.Id, StoredProcedureDocmapper.DeleteDocmapperContent);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error delete document content whit id {item.Id}: {JsonConvert.SerializeObject(ex)}");

                    throw;
                }
            }
        }

        private async Task UpdateDocumentContentAsync(List<DocmapperContent> documentContent)
        {
            foreach (DocmapperContent content in documentContent)
            {
                if (!await _documentContentRepository.ExistsAsync(content))
                {
                    await CreateDocumentContentAsync(content);
                }
                else
                {
                    UpdateDocmapperContentParameters parameters = new(content);

                    try
                    {
                        await _documentContentRepository.UpdateAsync(content, StoredProcedureDocmapper.UpdateDocmapperContent, parameters);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error update document content {JsonConvert.SerializeObject(content)}: {JsonConvert.SerializeObject(ex)}");

                        throw;
                    }
                }
            }
        }

        private async Task CreateDocumentContentAsync(DocmapperContent content)
        {
            CreateDocmapperContentParameters parameters = new(content);

            try
            {
                _ = await _documentContentRepository.CreateAsync(content, StoredProcedureDocmapper.AddNewDocmapperContent, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error add new document content {JsonConvert.SerializeObject(content)}: {JsonConvert.SerializeObject(ex)}");

                throw;
            }
        }
    }
}
