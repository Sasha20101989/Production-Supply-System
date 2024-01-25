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
    public class DocumentColumnSqlServerData : IDocumentColumnData
    {
        private readonly ISqlDataAccess _db;

        private readonly ILogger<DocumentColumnSqlServerData> _logger;

        private Lazy<Task<IEnumerable<DocumentColumn>>> _documentColumns;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="DocumentColumnSqlServerData"/> class.
        /// </summary>
        /// <param name="db">Служба доступа к данным SQL.</param>
        /// <param name="logger">Регистратор для отслеживания информации и ошибок.</param>
        public DocumentColumnSqlServerData(ISqlDataAccess db, ILogger<DocumentColumnSqlServerData> logger)
        {
            _db = db;
            _logger = logger;
            Refresh();
        }

        /// <inheritdoc />
        public void Refresh()
        {
            _documentColumns = new Lazy<Task<IEnumerable<DocumentColumn>>>(GetAllDocmapperColumnsAsync);
        }

        /// <inheritdoc />
        public async Task<DocumentColumn> GetByIdAsync(int docmapperColumnId)
        {
            IEnumerable<DocumentColumn> documentColumns = await _documentColumns.Value;

            return documentColumns.FirstOrDefault(c => c.DocmapperColumnId == docmapperColumnId);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DocumentColumn>> GetAllAsync()
        {
            return await _documentColumns.Value;
        }

        /// <inheritdoc />
        public async Task<DocumentColumn> AddDocumentColumnAsync(DocumentColumn documentColumn)
        {
            try
            {
                DynamicParameters parameters = new();
                parameters.Add("@ElementName", documentColumn.ElementName);
                parameters.Add("@SystemColumnName", documentColumn.SystemColumnName);

                _logger.LogInformation($"Create document column: {JsonConvert.SerializeObject(documentColumn)}");

                IEnumerable<DocumentColumn> result = await _db.LoadData<DocumentColumn>(
                    StoredProcedureDocmapper.AddNewDocmapperColumn,
                    parameters);

                DocumentColumn newColumn = result.First();

                documentColumn.DocmapperColumnId = newColumn.DocmapperColumnId;

                _logger.LogInformation($"Document column with id {documentColumn.DocmapperColumnId} created");

                Refresh();

                return documentColumn;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding document column: {JsonConvert.SerializeObject(documentColumn)} - {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }

        /// <inheritdoc />
        private async Task<IEnumerable<DocumentColumn>> GetAllDocmapperColumnsAsync()
        {
            try
            {
                _logger.LogInformation($"Get all document columns");

                IEnumerable<DocumentColumn> result = await _db.LoadData<DocumentColumn>(
                    StoredProcedureDocmapper.GetAllDocmapperColumns);

                _logger.LogInformation($"List of document columns received");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all document columns: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }
    }
}
