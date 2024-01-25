using System.ComponentModel;
using DAL.Attributes;

namespace DAL.Enums
{
    /// <summary>
    /// Перечисление, представляющее собой набор хранимых процедур для схемы базы данных "Docmapper".
    /// </summary>
    [ProcedureName(DatabaseSchemas.Docmapper)]
    public enum StoredProcedureDocmapper
    {
        /// <summary>
        /// Добавление новой карты документа/>.
        /// </summary>
        [Description("Add_New_Docmapper")]
        AddNewDocmapper,
        /// <summary>
        /// Получение всех карт документов/>.
        /// </summary>
        [Description("Get_All_Docmapper_Items")]
        GetAllDocmapperItems,
        /// <summary>
        /// Добавление содержимого карты документа/>.
        /// </summary>
        [Description("Add_New_Docmapper_Content")]
        AddNewDocmapperContent,

        /// <summary>
        /// Получение всех колонок для карт документов/>.
        /// </summary>
        [Description("Get_All_Docmapper_Columns")]
        GetAllDocmapperColumns,

        /// <summary>
        /// Получение всех колонок для контента карт документов/>.
        /// </summary>
        [Description("Get_All_Docmapper_Content_Items")]
        GetAllDocmapperContentItems,

        /// <summary>
        /// Обновление документа/>.
        /// </summary>
        [Description("Update_Docmapper_Item")]
        UpdateDocmapperItem,

        /// <summary>
        /// Удаление документа/>.
        /// </summary>
        [Description("Delete_Docmapper_Content")]
        DeleteDocmapperContent,

        /// <summary>
        /// Обновление документа/>.
        /// </summary>
        [Description("Update_Docmapper_Content")]
        UpdateDocmapperContent,

        /// <summary>
        /// Обновление документа/>.
        /// </summary>
        [Description("Add_New_Docmapper_Column")]
        AddNewDocmapperColumn,
    }
}
