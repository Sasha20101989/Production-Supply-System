using System.ComponentModel;
using DAL.Attributes;

namespace DAL.Enums
{
    /// <summary>
    /// Перечисление, представляющее собой набор хранимых процедур для схемы базы данных "Customs".
    /// </summary>
    [ProcedureName(DatabaseSchemas.Customs)]
    public enum StoredProcedureCustoms
    {
        /// <summary>
        /// Получение всех деталей.
        /// </summary>
        [Description("Get_All_Customs_Parts")]
        GetAllCustomsParts,
        /// <summary>
        /// Добавление новой детали.
        /// </summary>
        [Description("Add_New_Customs_Part")]
        AddNewCustomsPart,
        /// <summary>
        /// Получить все таможенные процедуры.
        /// </summary>
        [Description("Get_All_Customs_Clearance")]
        GetAllCustomsClearance,
        /// <summary>
        /// Добавление новой таможенной процедуры.
        /// </summary>
        [Description("Add_New_Customs_Clearance")]
        AddNewCustomsClearance
    }
}
