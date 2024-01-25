using System.Collections.Generic;
using System.Threading.Tasks;
using System;

using DAL.Data.Contracts;
using DAL.DbAccess.Contracts;

using Microsoft.Extensions.Logging;
using DAL.Models.Master;
using DAL.Enums;
using Newtonsoft.Json;
using System.Linq;

namespace DAL.Data.SqlServer
{
    public class ProcessStepSqlServerData : IProcessStepData
    {
        private readonly ISqlDataAccess _db;

        private readonly ILogger<ProcessStepSqlServerData> _logger;

        private Lazy<Task<IEnumerable<ProcessStep>>> _processSteps;

        private Dictionary<int, Process> _processMap;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ProcessStepSqlServerData"/> class.
        /// </summary>
        /// <param name="db">Служба доступа к данным SQL.</param>
        /// <param name="logger">Регистратор дл
        public ProcessStepSqlServerData(ISqlDataAccess db, ILogger<ProcessStepSqlServerData> logger)
        {
            _db = db;

            _logger = logger;

            _ = RefreshAsync();
        }

        /// <inheritdoc />
        public async Task RefreshAsync()
        {
            _processSteps = new Lazy<Task<IEnumerable<ProcessStep>>>(GetAllProcessStepsAsync);

            IEnumerable<Process> processes = await GetAllProcessesAsync();

            _processMap = processes.ToDictionary(p => p.ProcessId);

            IEnumerable<ProcessStep> steps = await _processSteps.Value;

            foreach (ProcessStep processStep in steps)
            {
                if (_processMap.TryGetValue(processStep.ProcessId, out Process process)) 
                {
                    processStep.Process = process;
                }
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ProcessStep>> GetProcessStepsByUserSectionAsync(int sectionId, AppProcess appProcess)
        {
            IEnumerable<ProcessStep> documents = await _processSteps.Value;

            return documents.Where(c => c.SectionId == sectionId && c.Process.ProcessName == appProcess);
        }

        private async Task<IEnumerable<ProcessStep>> GetAllProcessStepsAsync()
        {
            try
            {
                _logger.LogInformation($"Get all process steps");

                IEnumerable<ProcessStep> result = await _db.LoadData<ProcessStep>(
                    StoredProcedureMaster.GetAllProcessSteps);

                _logger.LogInformation($"List of process steps received");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all process steps: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }

        private async Task<IEnumerable<Process>> GetAllProcessesAsync()
        {
            try
            {
                _logger.LogInformation($"Get all processes");

                IEnumerable<Process> result = await _db.LoadData<Process>(
                    StoredProcedureMaster.GetAllProcesses);

                _logger.LogInformation($"List of processes received");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all processes: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }
    }
}
