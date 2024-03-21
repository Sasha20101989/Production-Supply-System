using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Contracts;
using BLL.Properties;
using DAL.Enums;
using DAL.Models;
using DAL.Models.Master;
using Microsoft.Extensions.Logging;

namespace BLL.Services
{
    public class ProcessService(
        IUserService userService,
        IStaticDataService staticDataService,
        IDocumentService documentService,
        ILogger<ProcessService> logger) : IProcessService
    {

        /// <inheritdoc />
        public async Task<IEnumerable<ProcessStep>> GetProcessStepsByUserSectionAsync(AppProcess appProcess)
        {
            logger.LogInformation($"{string.Format(Resources.LogProcessStepGet, appProcess)}");

            User user = userService.GetCurrentUser();

            IEnumerable<ProcessStep> steps = await staticDataService.GetProcessStepsByUserAsync(user);

            foreach (ProcessStep processStep in steps)
            {
                processStep.Section = await staticDataService.GetSectionByIdAsync(processStep.SectionId);

                processStep.Docmapper = await documentService.GetDocumentByIdAsync(processStep.DocmapperId);

                processStep.Process = await staticDataService.GetProcessByIdAsync(processStep.ProcessId);
            }

            IEnumerable<ProcessStep> filteredSteps = steps.Where(c => c.Process.ProcessName == appProcess);

            logger.LogInformation($"{string.Format(Resources.LogProcessStepGet, appProcess)} {Resources.Completed}");

            return filteredSteps;
        }
    }
}
