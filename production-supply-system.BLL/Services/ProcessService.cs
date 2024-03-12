using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Contracts;
using DAL.Data.Repositories.Contracts;
using DAL.Enums;
using DAL.Models;
using DAL.Models.Document;
using DAL.Models.Master;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace BLL.Services
{
    public class ProcessService : IProcessService
    {
        private readonly IRepository<ProcessStep> _processStepsRepository;
        private readonly IRepository<Process> _processRepository;
        private readonly IRepository<Section> _sectionRepository;
        private readonly IRepository<Docmapper> _documentRepository;
        private readonly IUserService _userService;
        private readonly ILogger<ProcessService> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ProcessService"/>.
        /// </summary>
        /// <param name="documentRepository">Репозиторий для доступа к информации о картах сопоставления данных для эксель.</param>
        /// /// <param name="logger">Регистратор для отслеживания информации и ошибок.</param>
        public ProcessService(
            IRepository<ProcessStep> processStepsRepository,
            IRepository<Process> processRepository,
            IRepository<Section> sectionRepository,
            IRepository<Docmapper> documentRepository,
            IUserService userService,
            ILogger<ProcessService> logger)
        {
            _processStepsRepository = processStepsRepository;
            _processRepository = processRepository;
            _sectionRepository = sectionRepository;
            _documentRepository = documentRepository;
            _userService = userService;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ProcessStep>> GetProcessStepsByUserSectionAsync(AppProcess appProcess)
        {
            User user = _userService.GetCurrentUser();

            if (user is null)
            {
                throw new Exception($"User not found.");
            }

            IEnumerable<ProcessStep> steps;

            try
            {
                steps = await _processStepsRepository.GetAllAsync();

                if (user.SectionId <= 0)
                {
                    throw new Exception("SectionId should be greater than 0");
                }

                steps = steps.Where(c => c.SectionId == user.SectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get process steps: {JsonConvert.SerializeObject(ex)}");

                throw;
            }

            foreach (ProcessStep processStep in steps)
            {
                try
                {
                    processStep.Section = await _sectionRepository.GetByIdAsync(processStep.SectionId);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error get section by id {processStep.SectionId}: {JsonConvert.SerializeObject(ex)}");

                    throw;
                }

                try
                {
                    processStep.Docmapper = await _documentRepository.GetByIdAsync(processStep.DocmapperId);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error get document by id {processStep.DocmapperId}: {JsonConvert.SerializeObject(ex)}");

                    throw;
                }

                try
                {
                    processStep.Process = await _processRepository.GetByIdAsync(processStep.ProcessId);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error get process by process id {processStep.ProcessId}: {JsonConvert.SerializeObject(ex)}");

                    throw;
                }
            }

            return steps.Where(c => c.Process.ProcessName == appProcess);
        }
    }
}
