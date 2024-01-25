using System.ComponentModel;

using DAL.Attributes;

namespace DAL.Enums
{
    /// <summary>
    /// Перечисление, представляющее собой набор хранимых процедур для схемы базы данных "Master".
    /// </summary>
    [ProcedureName(DatabaseSchemas.Master)]
    public enum StoredProcedureMaster
    {
        /// <summary>
        /// Получение всех шагов процессов/>.
        /// </summary>
        [Description("Get_All_Process_Steps")]
        GetAllProcessSteps,
        /// <summary>
        /// Получение всех шагов/>.
        /// </summary>
        [Description("Get_All_Processes")]
        GetAllProcesses
    }
}
