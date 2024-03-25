using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BLL.Contracts;
using BLL.Properties;

using Microsoft.Extensions.Logging;

using production_supply_system.EntityFramework.DAL.Enums;
using production_supply_system.EntityFramework.DAL.MasterProcessContext.Models;
using production_supply_system.EntityFramework.DAL.Models.UsersSchema;

namespace BLL.Services
{
    public class ProcessService(IUserService userService) : IProcessService
    {
        /// <inheritdoc />
        public async Task<IEnumerable<ProcessesStep>> GetProcessStepsByUserSectionAsync(AppProcess appProcess)
        {
            User user = await userService.GetCurrentUser(Environment.UserName);

            return user.Section.ProcessesSteps
                .Where(c => c.Process.ProcessName == appProcess.ToString());
        }
    }
}
