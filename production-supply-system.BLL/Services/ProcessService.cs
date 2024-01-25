using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Contracts;
using DAL.Data.Repositories.Contracts;
using DAL.Enums;
using DAL.Models.Master;

namespace BLL.Services
{
    public class ProcessService : IProcessService
    {
        private readonly IProcessStepsRepository _processStepsRepository;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ProcessService"/>.
        /// </summary>
        /// <param name="documentMapperRepository">Репозиторий для доступа к информации о картах сопоставления данных для эксель.</param>
        public ProcessService(IProcessStepsRepository processStepsRepository)
        {
            _processStepsRepository = processStepsRepository;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ProcessStep>> GetProcessStepsByUserSectionAsync(int sectionId, AppProcess appProcess)
        {
            return await _processStepsRepository.GetProcessStepsByUserSectionAsync(sectionId, appProcess);
        }
    }
}
