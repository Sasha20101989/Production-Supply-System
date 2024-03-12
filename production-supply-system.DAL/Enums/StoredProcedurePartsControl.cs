using System.ComponentModel;
using DAL.Attributes;

namespace DAL.Enums
{
    /// <summary>
    /// Перечисление, представляющее собой набор хранимых процедур для схемы базы данных "Partscontrol".
    /// </summary>
    [ProcedureName(DatabaseSchemas.Partscontrol)]
    public enum StoredProcedurePartscontrol
    {
        /// <summary>
        /// Получение всех заказов/>.
        /// </summary>
        [Description("Get_All_Purchase_Orders")]
        GetAllPurchaseOrders,
        /// <summary>
        /// Получение всех типов заказов/>.
        /// </summary>
        [Description("Get_All_Purchase_Order_Types")]
        GetAllPurchaseOrderTypes,
    }
}
