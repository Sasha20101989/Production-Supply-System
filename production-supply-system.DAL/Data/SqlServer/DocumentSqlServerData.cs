using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Реализация SQL Server для взаимодействия с картами сопоставления ячеек эксель.
    /// </summary>
    public class DocumentSqlServerData : IDocumentData
    {
        private readonly ISqlDataAccess _db;

        private readonly ILogger<DocumentSqlServerData> _logger;

        private Lazy<Task<IEnumerable<Document>>> _documents;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="DocumentSqlServerData"/> class.
        /// </summary>
        /// <param name="db">Служба доступа к данным SQL.</param>
        /// <param name="logger">Регистратор для отслеживания информации и ошибок.</param>
        public DocumentSqlServerData(ISqlDataAccess db, ILogger<DocumentSqlServerData> logger)
        {
            _db = db;
            _logger = logger;
            Refresh();
        }

        /// <inheritdoc />
        public void Refresh()
        {
            _documents = new Lazy<Task<IEnumerable<Document>>>(GetAllDocumentMapsAsync);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            return await _documents.Value;
        }

        /// <inheritdoc />
        public async Task<Document> GetDocumentByIdAsync(int mapId)
        {
            IEnumerable<Document> documents = await _documents.Value;

            return documents.FirstOrDefault(c => c.DocmapperId == mapId);
        }

        /// <inheritdoc />
        public async Task<Document> CreateDocumentAsync(Document document)
        {
            try
            {
                DynamicParameters parameters = new();
                parameters.Add("@DocmapperName", document.DocmapperName);
                parameters.Add("@DefaultFolder", document.DefaultFolder);
                parameters.Add("@SheetName", document.SheetName);
                parameters.Add("@FirstDataRow", document.FirstDataRow);

                _logger.LogInformation($"Create document: {JsonConvert.SerializeObject(document)}");

                IEnumerable<Document> result = await _db.LoadData<Document>(
                    StoredProcedureDocmapper.AddNewDocmapper,
                    parameters);

                Document newDocument = result.First();

                document.DocmapperId = newDocument.DocmapperId;
                document.IsActive = newDocument.IsActive;

                _logger.LogInformation($"Document created with id {document.DocmapperId}");

                Refresh();

                return document;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding document: {JsonConvert.SerializeObject(document)} - {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateDocumentAsync(Document document)
        {
            try
            {
                DynamicParameters parameters = new();

                parameters.Add("@DocmapperId", document.DocmapperId);
                parameters.Add("@DocmapperName", document.DocmapperName);
                parameters.Add("@DefaultFolder", document.DefaultFolder);
                parameters.Add("@SheetName", document.SheetName);
                parameters.Add("@FirstDataRow", document.FirstDataRow);
                parameters.Add("@IsActive", document.IsActive);

                _logger.LogInformation($"Update document with id {document.DocmapperId} " +
                    $"-> Updated information: {JsonConvert.SerializeObject(document)}");

                await _db.SaveData(
                    StoredProcedureDocmapper.UpdateDocmapperItem, parameters);

                _logger.LogInformation($"Document with id {document.DocmapperId} updated");

                Refresh();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating document with id {document.DocmapperId}: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }

        private async Task<IEnumerable<Document>> GetAllDocumentMapsAsync()
        {
            try
            {
                _logger.LogInformation($"Get all document maps");

                IEnumerable<Document> result = await _db.LoadData<Document>(
                    StoredProcedureDocmapper.GetAllDocmapperItems);

                _logger.LogInformation($"List of document maps received");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all document maps: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }
    }
}
