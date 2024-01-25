using System.ComponentModel;
using DAL.Attributes;

namespace DAL.Enums
{
    /// <summary>
    /// Перечисление, представляющее собой набор хранимых процедур для схемы базы данных "Dbo".
    /// </summary>
    [ProcedureName(DatabaseSchemas.Dbo)]
    public enum StoredProcedureDbo
    {
        /// <summary>
        /// Обновление документа/>.
        /// </summary>
        [Description("Get_All_Sections")]
        GetAllSections,
    }
}
