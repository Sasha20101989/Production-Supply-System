using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Contracts;
using BLL.Properties;

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
    /// <remarks>
    /// Инициализирует новый экземпляр класса <see cref="DocumentService"/>.
    /// </remarks>
    /// <param name="documentMapperRepository">Репозиторий для доступа к информации о картах сопоставления данных для эксель.</param>
    /// <param name="logger">Регистратор для отслеживания информации и ошибок.</param>
    public class DocumentService(
        IRepository<Docmapper> documentMapperRepository,
        IRepository<DocmapperColumn> documentColumnRepository,
        IRepository<DocmapperContent> documentContentRepository,
        ILogger<DocumentService> logger) : IDocumentService
    {

        /// <inheritdoc />
        public async Task<Docmapper> CreateDocumentAsync(Docmapper document, List<DocmapperContent> documentContent)
        {
            CreateDocmapperParameters parameters = new(document);

            try
            {
                logger.LogInformation($"{Resources.LogDocmapperAdd}: '{JsonConvert.SerializeObject(document)}'");

                Docmapper newDocument = await documentMapperRepository.CreateAsync(document, StoredProcedureDocmapper.AddNewDocmapper, parameters);

                foreach (DocmapperContent contentItem in documentContent)
                {
                    contentItem.DocmapperId = newDocument.Id;

                    contentItem.Docmapper = newDocument;

                    await CreateDocumentContentAsync(contentItem);
                }

                newDocument.DocmapperContents = documentContent;

                logger.LogInformation($"{Resources.LogDocmapperAdd} {Resources.Completed}");

                RefreshDocmappers();

                return newDocument;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogDocmapperAdd}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Docmapper>> GetAllDocumentsAsync()
        {
            try
            {
                logger.LogInformation($"{Resources.LogDocmapperGet}");

                IEnumerable<Docmapper> docmappers = await documentMapperRepository.GetAllAsync();

                logger.LogInformation($"{Resources.LogDocmapperGet} {Resources.Completed}");

                return docmappers;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogDocmapperGet}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<Docmapper> GetDocumentByIdAsync(int mapId)
        {
            try
            {
                logger.LogInformation($"{string.Format(Resources.LogDocmapperGetById, mapId)}");

                Docmapper document = await documentMapperRepository.GetByIdAsync(mapId);

                logger.LogInformation($"{string.Format(Resources.LogDocmapperGetById, mapId)} {Resources.Completed}");

                if (document is not null)
                {
                    document.DocmapperContents = await GetAllDocumentContentItemsByIdAsync(mapId);
                }

                return document;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {string.Format(Resources.LogDocmapperGetById, mapId)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DocmapperColumn>> GetAllColumnsAsync()
        {
            try
            {
                logger.LogInformation($"{Resources.LogDocmapperColumnGet}");

                IEnumerable<DocmapperColumn> columns = await documentColumnRepository.GetAllAsync();

                logger.LogInformation($"{Resources.LogDocmapperColumnGet} {Resources.Completed}");

                return columns;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogDocmapperColumnGet}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task UpdateDocumentAsync(Docmapper document, List<DocmapperContent> documentContent)
        {
            UpdateDocmapperParameters parameters = new(document);

            try
            {
                logger.LogInformation($"{Resources.LogDocmapperUpdate}");

                await documentMapperRepository.UpdateAsync(StoredProcedureDocmapper.UpdateDocmapperItem, parameters);

                logger.LogInformation($"{Resources.LogDocmapperUpdate} {Resources.Completed}");
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogDocmapperUpdate}: {JsonConvert.SerializeObject(document)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }

            IEnumerable<DocmapperContent> cachedContentItems = await GetAllDocumentContentItemsByIdAsync(document.Id);

            await DeleteExistingItemsAsync(documentContent, cachedContentItems);

            await UpdateOrCreateDocumentContentAsync(documentContent);

            RefreshDocmappers();

            RefreshDocmapperContent();
        }

        /// <inheritdoc />
        public async Task<DocmapperColumn> AddDocumentColumnAsync(DocmapperColumn documentColumn)
        {
            try
            {
                CreateDocmapperColumnParameters parameters = new(documentColumn);

                logger.LogInformation($"{Resources.LogDocmapperColumnAdd}");

                DocmapperColumn column = await documentColumnRepository.CreateAsync(documentColumn, StoredProcedureDocmapper.AddNewDocmapperColumn, parameters);

                logger.LogInformation($"{Resources.LogDocmapperColumnAdd} {Resources.Completed}");

                RefreshDocmapperColumns();

                return column;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogDocmapperColumnAdd}: {JsonConvert.SerializeObject(documentColumn)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<List<DocmapperContent>> GetAllDocumentContentItemsByIdAsync(int mapId)
        {
            try
            {
                logger.LogInformation($"{string.Format(Resources.LogDocmapperContentGetByDocmapperId, mapId)}");

                IEnumerable<DocmapperContent> documentContent = (await documentContentRepository.GetAllAsync())
                                                                        .Where(con => con.DocmapperId == mapId)
                                                                        .OrderBy(dc => dc.ColumnNr);

                logger.LogInformation($"{string.Format(Resources.LogDocmapperContentGetByDocmapperId, mapId)} {Resources.Completed}");

                foreach (DocmapperContent item in documentContent)
                {
                    item.DocmapperColumn = await GetDocumentColumnByIdAsync(item);
                }

                return documentContent?.ToList();

            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {string.Format(Resources.LogDocmapperContentGetByDocmapperId, mapId)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        private void RefreshDocmappers()
        {
            try
            {
                logger.LogInformation($"{Resources.LogDocmapperRefresh}");

                documentMapperRepository.RefreshData();

                logger.LogInformation($"{Resources.LogDocmapperRefresh} {Resources.Completed}");
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogDocmapperRefresh}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        private void RefreshDocmapperContent()
        {
            try
            {
                logger.LogInformation($"{Resources.LogDocmapperContentRefresh}");

                documentContentRepository.RefreshData();

                logger.LogInformation($"{Resources.LogDocmapperContentRefresh} {Resources.Completed}");
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogDocmapperContentRefresh}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        private void RefreshDocmapperColumns()
        {
            try
            {
                logger.LogInformation($"{Resources.LogDocmapperColumnRefresh}");

                documentContentRepository.RefreshData();

                logger.LogInformation($"{Resources.LogDocmapperColumnRefresh} {Resources.Completed}");
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogDocmapperColumnRefresh}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        private async Task DeleteDocumentContentItemByIdAsync(DocmapperContent item)
        {
            try
            {
                logger.LogInformation($"{string.Format(Resources.LogDocmapperContentDeleteById, item.Id)}");

                await documentContentRepository.RemoveAsync(item.Id, StoredProcedureDocmapper.DeleteDocmapperContent);

                logger.LogInformation($"{string.Format(Resources.LogDocmapperContentDeleteById, item.Id)} {Resources.Completed}");
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {string.Format(Resources.LogDocmapperContentDeleteById, item.Id)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        private async Task<DocmapperColumn> GetDocumentColumnByIdAsync(DocmapperContent item)
        {
            try
            {
                logger.LogInformation($"{string.Format(Resources.LogDocmapperColumnGetById, item.DocmapperColumnId)}");

                DocmapperColumn column = await documentColumnRepository.GetByIdAsync(item.DocmapperColumnId);

                logger.LogInformation($"{string.Format(Resources.LogDocmapperColumnGetById, item.DocmapperColumnId)} {Resources.Completed}");

                return column;
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {string.Format(Resources.LogDocmapperColumnGetById, item.DocmapperColumnId)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        private async Task DeleteExistingItemsAsync(List<DocmapperContent> documentContent, IEnumerable<DocmapperContent> cachedContentItems)
        {
            logger.LogInformation($"{Resources.LogDocmapperContentGetMissing}");

            IEnumerable<DocmapperContent> missingContent = cachedContentItems
                .Where(ca => !documentContent.Any(dc => dc.Id == ca.Id));

            logger.LogInformation($"{Resources.LogDocmapperContentGetMissing} {Resources.Completed}");

            foreach (DocmapperContent item in missingContent)
            {
                await DeleteDocumentContentItemByIdAsync(item);
            }
        }

        private async Task UpdateOrCreateDocumentContentAsync(List<DocmapperContent> documentContent)
        {
            foreach (DocmapperContent content in documentContent)
            {
                if (!await documentContentRepository.ExistsAsync(content))
                {
                    await CreateDocumentContentAsync(content);
                }
                else
                {
                    try
                    {
                        UpdateDocmapperContentParameters parameters = new(content);

                        logger.LogInformation($"{Resources.LogDocmapperContentUpdate}");

                        await documentContentRepository.UpdateAsync(StoredProcedureDocmapper.UpdateDocmapperContent, parameters);

                        logger.LogInformation($"{Resources.LogDocmapperContentUpdate} {Resources.Completed}");
                    }
                    catch (Exception ex)
                    {
                        string message = $"{Resources.Error} {Resources.LogDocmapperContentUpdate}: {JsonConvert.SerializeObject(content)}: {JsonConvert.SerializeObject(ex)}";

                        logger.LogError(message);

                        throw new Exception(message);
                    }
                }
            }
        }

        private async Task CreateDocumentContentAsync(DocmapperContent content)
        {
            try
            {
                CreateDocmapperContentParameters parameters = new(content);

                logger.LogInformation($"{Resources.LogDocmapperContentAdd}");

                _ = await documentContentRepository.CreateAsync(content, StoredProcedureDocmapper.AddNewDocmapperContent, parameters);

                logger.LogInformation($"{Resources.LogDocmapperContentAdd} {Resources.Completed}");
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogDocmapperContentAdd} {JsonConvert.SerializeObject(content)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }
    }
}
