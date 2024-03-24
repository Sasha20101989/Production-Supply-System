using System.Collections.Generic;
using System.Threading.Tasks;

using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;
using production_supply_system.EntityFramework.DAL.Enums;

namespace BLL.Contracts
{
    public interface IExportProceduresService
    {
        Task ExportTracingForPartner2(AppProcess processName, Steps step, string filePath, string sheetName, List<DocmapperContent> content);
    }
}
