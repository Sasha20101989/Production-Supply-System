using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BLL.Contracts;
using BLL.Properties;

using Microsoft.Extensions.Logging;

using production_supply_system.EntityFramework.DAL.Enums;
using production_supply_system.EntityFramework.DAL.Models.MasterSchema;
using production_supply_system.EntityFramework.DAL.Models.UsersSchema;

namespace BLL.Services
{
    public class ProcessService(
        IUserService userService,
        IStaticDataService staticDataService,
        IDocumentService documentService,
        ILogger<ProcessService> logger) : IProcessService
    {

        /// <inheritdoc />
        public async Task<IEnumerable<ProcessesStep>> GetProcessStepsByUserSectionAsync(AppProcess appProcess)
        {
            logger.LogInformation($"{string.Format(Resources.LogProcessStepGet, appProcess)}");

            User user = await userService.GetCurrentUser(Environment.UserName);

            IEnumerable<ProcessesStep> steps = await staticDataService.GetProcessStepsByUserAsync(user);

            foreach (ProcessesStep processStep in steps)
            {
                processStep.Section = await staticDataService.GetSectionByIdAsync(processStep.SectionId);

                processStep.Docmapper = await documentService.GetDocumentByIdAsync(processStep.DocmapperId);

                processStep.Process = await staticDataService.GetProcessByIdAsync(processStep.ProcessId);
            }

            IEnumerable<ProcessesStep> filteredSteps = steps.Where(c => c.Process.ProcessName == appProcess);

            logger.LogInformation($"{string.Format(Resources.LogProcessStepGet, appProcess)} {Resources.Completed}");

            return filteredSteps;
        }
    }
}
