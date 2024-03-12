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
        /// <summary>
        /// Получение всех вариантов/>.
        /// </summary>
        [Description("Get_All_Model_Variants")]
        GetAllModelVariants,
        /// <summary>
        /// Получение всех деталей/>.
        /// </summary>
        [Description("Get_All_Bom_Parts")]
        GetAllBomParts,
        /// <summary>
        /// Добавление новой детали/>.
        /// </summary>
        [Description("Add_New_Bom_Part")]
        AddNewBomPart
    }
}
