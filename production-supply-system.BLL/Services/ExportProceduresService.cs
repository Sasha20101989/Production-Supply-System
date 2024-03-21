using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLL.Contracts;
using BLL.Properties;

using DAL.Enums;
using DAL.Models.Document;

using Microsoft.Extensions.Logging;

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
