using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Data.Contracts;
using DAL.Data.Repositories.Contracts;
using DAL.Enums;
using DAL.Models.Master;

namespace DAL.Data.Repositories
{
    /// <summary>
    /// Реализация репозитория для взаимодействия с процессами приложения.
    /// </summary>
    public class ProcessStepsRepository : IProcessStepsRepository
    {
        private readonly IProcessStepData _processStepData;

        private readonly ISectionData _sectionData;

        private readonly IDocumentMapperRepository _documentMapperRepository;

        public ProcessStepsRepository(IProcessStepData processStepData, ISectionData sectionData, IDocumentMapperRepository documentMapperRepository)
        {
            _processStepData = processStepData;

            _sectionData = sectionData;

            _documentMapperRepository = documentMapperRepository;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ProcessStep>> GetProcessStepsByUserSectionAsync(int sectionId, AppProcess appProcess)
        {
            IEnumerable<ProcessStep> steps = await _processStepData.GetProcessStepsByUserSectionAsync(sectionId, appProcess);

            foreach (ProcessStep processStep in steps)
            {
                processStep.Section = await _sectionData.GetSectionByIdAsync(processStep.SectionId);
                processStep.Document = await _documentMapperRepository.GetDocumentByIdAsync(processStep.DocmapperId);
            }

            return steps;
        }
    }
}