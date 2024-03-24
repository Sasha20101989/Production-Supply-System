using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BLL.Contracts;
using BLL.Properties;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Context;
using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;

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
    public class DocumentService(DocmapperContext db, ILogger<DocumentService> logger) : IDocumentService
    {
        /// <inheritdoc />
        public async Task CreateDocumentAsync(Docmapper document)
        {
            try
            {
                logger.LogInformation($"{Resources.LogDocmapperAdd}: '{JsonConvert.SerializeObject(document)}'");

                db.Entry(document).State = EntityState.Added;

                _ = await db.SaveChangesAsync();

                foreach (DocmapperContent contentItem in document.DocmapperContents)
                {
                    contentItem.DocmapperId = document.Id;

                    await CreateDocumentContentAsync(contentItem);
                }

                logger.LogInformation($"{Resources.LogDocmapperAdd} {Resources.Completed}");
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogDocmapperAdd}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        /// <inheritdoc />
        public async Task<List<Docmapper>> GetFilteredDocumentsAsync(string docmapperName)
        {
            try
            {
                logger.LogInformation($"{Resources.LogDocmapperGet}");

                List<Docmapper> docmappers = await db.Docmappers
                    .AsNoTrackingWithIdentityResolution()
                    .Where(document => EF.Functions.Like(document.DocmapperName, $"%{docmapperName}%"))
                    .ToListAsync();

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
        public async Task<List<Docmapper>> GetAllDocumentsAsync()
        {
            try
            {
                logger.LogInformation($"{Resources.LogDocmapperGet}");

                List<Docmapper> docmappers = await db.Docmappers
                    .AsNoTracking()
                    .Include(d => d.DocmapperContents)
                    .ThenInclude(d => d.DocmapperColumn)
                    .ToListAsync();

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
        public async Task UpdateDocumentContentsAsync(ICollection<DocmapperContent> docmapperContents, int docmapperId)
        {
            await RemoveOldContentAsync(docmapperContents, docmapperId);

            await UpdateOrCreateDocumentContentAsync([.. docmapperContents]);
        }

        /// <inheritdoc />
        public async Task UpdateDocumentAsync(Docmapper document)
        {
            try
            {
                Docmapper documentFind = await db.Docmappers.FindAsync(document.Id);

                if (documentFind is not null)
                {
                    logger.LogInformation($"{Resources.LogDocmapperUpdate}");

                    documentFind.DocmapperName = document.DocmapperName;
                    documentFind.SheetName = document.SheetName;
                    documentFind.FirstDataRow = document.FirstDataRow;
                    documentFind.DefaultFolder = document.DefaultFolder;
                    documentFind.IsActive = document.IsActive;

                    _ = await db.SaveChangesAsync();

                    logger.LogInformation($"{Resources.LogDocmapperUpdate} {Resources.Completed}");
                }
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogDocmapperUpdate}: {JsonConvert.SerializeObject(document)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        private async Task RemoveOldContentAsync(ICollection<DocmapperContent> docmapperContents, int docmapperId)
        {
            logger.LogInformation($"{Resources.LogDocmapperContentGetMissing}");

            List<DocmapperContent> oldContent = await db.DocmapperContents
                .Where(ca => !docmapperContents.Select(dc => dc.Id).Contains(ca.Id) && ca.DocmapperId == docmapperId)
                .ToListAsync();

            logger.LogInformation($"{Resources.LogDocmapperContentGetMissing} {Resources.Completed}");

            foreach (DocmapperContent content in oldContent)
            {
                _ = db.DocmapperContents.Remove(content);

                _ = await db.SaveChangesAsync();
            }
        }

        private async Task UpdateOrCreateDocumentContentAsync(List<DocmapperContent> documentContent)
        {
            foreach (DocmapperContent content in documentContent)
            {
                DocmapperContent contentFind = await db.DocmapperContents.FindAsync(content.Id);

                if (contentFind is not null)
                {
                    try
                    {
                        if (contentFind.ColumnNr != content.ColumnNr && contentFind.RowNr != content.RowNr)
                        {
                            logger.LogInformation($"{Resources.LogDocmapperContentUpdate}");

                            contentFind.ColumnNr = content.ColumnNr;
                            contentFind.RowNr = content.RowNr;

                            _ = await db.SaveChangesAsync();

                            logger.LogInformation($"{Resources.LogDocmapperContentUpdate} {Resources.Completed}");
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = $"{Resources.Error} {Resources.LogDocmapperContentUpdate}: {JsonConvert.SerializeObject(content)}: {JsonConvert.SerializeObject(ex)}";

                        logger.LogError(message);

                        throw new Exception(message);
                    }
                }
                else
                {
                    await CreateDocumentContentAsync(content);
                }
            }
        }

        /// <inheritdoc />
        public async Task<Docmapper> GetDocumentByIdAsync(int mapId)
        {
            try
            {
                logger.LogInformation($"{string.Format(Resources.LogDocmapperGetById, mapId)}");

                Docmapper document = await db.Docmappers
                    .AsNoTracking()
                    .Include(d => d.DocmapperContents)
                    .ThenInclude(dc => dc.DocmapperColumn)
                    .FirstOrDefaultAsync(d => d.Id == mapId);

                logger.LogInformation($"{string.Format(Resources.LogDocmapperGetById, mapId)} {Resources.Completed}");

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

                List<DocmapperColumn> columns = await db.DocmapperColumns
                    .OrderBy(dc => dc.ElementName)
                    .ToListAsync();

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
        public async Task AddDocumentColumnAsync(DocmapperColumn documentColumn)
        {
            try
            {
                logger.LogInformation($"{Resources.LogDocmapperColumnAdd}");

                _ = await db.DocmapperColumns.AddAsync(documentColumn);

                _ = await db.SaveChangesAsync();

                logger.LogInformation($"{Resources.LogDocmapperColumnAdd} {Resources.Completed}");
            }
            catch (Exception ex)
            {
                string message = $"{Resources.Error} {Resources.LogDocmapperColumnAdd}: {JsonConvert.SerializeObject(documentColumn)}: {JsonConvert.SerializeObject(ex)}";

                logger.LogError(message);

                throw new Exception(message);
            }
        }

        private async Task CreateDocumentContentAsync(DocmapperContent content)
        {
            try
            {
                logger.LogInformation($"{Resources.LogDocmapperContentAdd}");

                db.Entry(content).State = EntityState.Added;

                _ = await db.SaveChangesAsync();

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
