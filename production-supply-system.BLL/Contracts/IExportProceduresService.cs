using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL.Enums;
using DAL.Models.Document;

namespace BLL.Contracts
{
    public interface IExportProceduresService
    {
        Task ExportTracingForPartner2(AppProcess processName, Steps step, string filePath, string sheetName, List<DocmapperContent> content);
    }
}
