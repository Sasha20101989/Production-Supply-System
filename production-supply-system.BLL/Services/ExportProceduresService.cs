using System.Collections.Generic;
using System.Threading.Tasks;

using BLL.Contracts;
using BLL.Properties;

using Microsoft.Extensions.Logging;

using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;
using production_supply_system.EntityFramework.DAL.Enums;

namespace BLL.Services
{
    public class ExportProceduresService(
        IExcelService excelService,
        IDeliveryService deliveryService,
        ILogger<ExportProceduresService> logger) : IExportProceduresService
    {
        public async Task ExportTracingForPartner2(AppProcess processName, Steps step, string filePath, string sheetName, List<DocmapperContent> content)
        {
            logger.LogInformation(string.Format(Resources.LogExportFile, filePath));

            excelService.ExportFile(
                await deliveryService.GetAllTracingForPartner2ToExport(content),
                filePath,
                sheetName);

            logger.LogInformation($"{string.Format(Resources.LogExportFile, filePath)} {Resources.Completed}");
        }
    }
}
