using System.ComponentModel;
using DAL.Attributes;

namespace DAL.Enums
{
    /// <summary>
    /// Перечисление, представляющее собой набор хранимых процедур для схемы базы данных "Planning".
    /// </summary>
    [ProcedureName(DatabaseSchemas.Planning)]
    public enum StoredProcedurePlanning
    {
        /// <summary>
        /// Добавление нового сопоставления vin и контейнера.
        /// </summary>
        [Description("Add_New_Vin_Container")]
        AddNewVinContainer,
        /// <summary>
        /// Получение всех вин-контейнеров.
        /// </summary>
        [Description("Get_All_Vin_Containers")]
        GetAllVinContainers
    }
}
