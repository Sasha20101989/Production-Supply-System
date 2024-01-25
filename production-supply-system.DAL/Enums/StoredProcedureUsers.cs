using System.ComponentModel;

using DAL.Attributes;

namespace DAL.Enums
{
    /// <summary>
    /// Перечисление, представляющее собой набор хранимых процедур для схемы базы данных "Users".
    /// </summary>
    [ProcedureName(DatabaseSchemas.Users)]
    public enum StoredProcedureUsers
    {
        /// <summary>
        /// Получение пользователя по учетной записи.
        /// </summary>
        [Description("Get_User_By_Account")]
        GetUserByAccount,
    }
}