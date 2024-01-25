using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data.Contracts;
using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models.Docmapper;
using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DAL.Data.SqlServer
{
    public class DocumentContentSqlServerData : IDocumentContentData
    {
        private readonly ISqlDataAccess _db;

        private readonly ILogger<DocumentColumnSqlServerData> _logger;

        private Lazy<Task<IEnumerable<DocumentContent>>> _content;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="DocumentContentSqlServerData"/> class.
        /// </summary>
        /// <param name="db">Служба доступа к данным SQL.</param>
        /// <param name="logger">Регистратор для отслеживания информации и ошибок.</param>
        public DocumentContentSqlServerData(ISqlDataAccess db, ILogger<DocumentColumnSqlServerData> logger)
        {
            _db = db;

            _logger = logger;

            Refresh();
        }

        /// <inheritdoc />
        public void Refresh()
        {
            _content = new Lazy<Task<IEnumerable<DocumentContent>>>(GetAllDocmapperContentItemsAsync);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DocumentContent>> GetAllAsync()
        {
            return await _content.Value;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DocumentContent>> GetAllDocumentContentItemsByIdAsync(int mapId)
        {
            IEnumerable<DocumentContent> content = await _content.Value;

            return content.Where(c => c.DocmapperId == mapId);
        }

        /// <inheritdoc />
        public async Task CreateDocumentContentAsync(DocumentContent content)
        {
            try
            {
                DynamicParameters parameters = new();
                parameters.Add("@DocmapperId", content.DocmapperId);
                parameters.Add("@DocmapperColumnId", content.DocmapperColumnId);
                parameters.Add("@RowNumber", content.RowNumber);
                parameters.Add("@ColumnNumber", content.ColumnNumber);

                _logger.LogInformation($"Create document content: {JsonConvert.SerializeObject(content)}");

                IEnumerable<DocumentContent> result = await _db.LoadData<DocumentContent>(
                    StoredProcedureDocmapper.AddNewDocmapperContent,
                parameters);

                DocumentContent newContent = result.First();

                content.DocmapperContentId = newContent.DocmapperContentId;

                _logger.LogInformation($"Document with id {content.DocmapperContentId} created");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding document content: {JsonConvert.SerializeObject(content)} - {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<DocumentContent> GetByIdAsync(int docmapperContentId)
        {
            IEnumerable<DocumentContent> content = await _content.Value;

            return content.FirstOrDefault(c => c.DocmapperContentId == docmapperContentId);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(DocumentContent content)
        {
            return await GetByIdAsync(content.DocmapperContentId) is not null;
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int docmapperContentId)
        {
            try
            {
                DynamicParameters parameters = new();
                parameters.Add("@DocmapperContentId", docmapperContentId);

                _logger.LogInformation($"Delete document content with id {docmapperContentId}");

                await _db.SaveData(
                  StoredProcedureDocmapper.DeleteDocmapperContent,
                  parameters);

                _logger.LogInformation($"Document with id {docmapperContentId} deleted");

                Refresh();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting document content with id {docmapperContentId}: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateAsync(DocumentContent content)
        {
            try
            {
                DynamicParameters parameters = new();
                parameters.Add("@DocmapperContentId", content.DocmapperContentId);
                parameters.Add("@RowNumber", content.RowNumber);
                parameters.Add("@ColumnNumber", content.ColumnNumber);

                _logger.LogInformation($"Update document content with id {content.DocmapperContentId} " +
                    $"-> Updated information: {JsonConvert.SerializeObject(content)}");

                await _db.SaveData(
                  StoredProcedureDocmapper.UpdateDocmapperContent,
                  parameters);

                _logger.LogInformation($"Document content with id {content.DocmapperContentId} updated");

                Refresh();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating document content with id {content.DocmapperContentId}: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }

        private async Task<IEnumerable<DocumentContent>> GetAllDocmapperContentItemsAsync()
        {
            try
            {
                _logger.LogInformation($"Get all content items for document maps");

                IEnumerable<DocumentContent> result = await _db.LoadData<DocumentContent>(
                    StoredProcedureDocmapper.GetAllDocmapperContentItems);

                _logger.LogInformation($"List of content items for document maps received");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all content items for document maps: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }
    }
}
