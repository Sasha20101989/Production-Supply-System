using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data.Contracts;
using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DAL.Data.SqlServer
{
    public class SectionSqlServerData : ISectionData
    {
        private readonly ISqlDataAccess _db;

        private readonly ILogger<SectionSqlServerData> _logger;

        private Lazy<Task<IEnumerable<Section>>> _sections;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="SectionSqlServerData"/> class.
        /// </summary>
        /// <param name="db">Служба доступа к данным SQL.</param>
        /// <param name="logger">Регистратор для отслеживания информации и ошибок.</param>
        public SectionSqlServerData(ISqlDataAccess db, ILogger<SectionSqlServerData> logger)
        {
            _db = db;

            _logger = logger;

            Refresh();
        }

        /// <inheritdoc />
        public void Refresh()
        {
            _sections = new Lazy<Task<IEnumerable<Section>>>(GetAllSectionsAsync);
        }

        /// <inheritdoc />
        public async Task<Section> GetSectionByIdAsync(int sectionId)
        {
            IEnumerable<Section> sections = await _sections.Value;

            return sections.FirstOrDefault(c => c.SectionId == sectionId);
        }

        private async Task<IEnumerable<Section>> GetAllSectionsAsync()
        {
            try
            {
                _logger.LogInformation($"Get all section");

                IEnumerable<Section> result = await _db.LoadData<Section>(
                    StoredProcedureDbo.GetAllSections);

                _logger.LogInformation($"List of sections received");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all sections: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }
    }
}
