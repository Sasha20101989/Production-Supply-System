using System.ComponentModel;

using DAL.Attributes;

namespace DAL.Enums
{
    /// <summary>
    /// Перечисление, представляющее собой набор хранимых процедур для схемы базы данных "Inbound".
    /// </summary>
    [ProcedureName(DatabaseSchemas.Inbound)]
    public enum StoredProcedureInbound
    {
        /// <summary>
        /// Получение пользователя всех лотов.
        /// </summary>
        [Description("Get_All_Lot_Items")]
        GetAllLotItems
    }
}
