using System;
using System.Threading.Tasks;
using DAL.Data.Contracts;
using DAL.Data.Repositories.Contracts;
using DAL.Models;

namespace DAL.Data.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly ISectionData _sectionData;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="SectionRepository"/> class.
        /// </summary>
        /// <param name="sectionData">Источник данных для информации о секции.</param>
        public SectionRepository(ISectionData sectionData)
        {
            _sectionData = sectionData;
        }

        /// <inheritdoc />
        public async Task<Section> GetSectionByIdAsync(int sectionId)
        {
            return await _sectionData.GetSectionByIdAsync(sectionId);
        }
    }
}
