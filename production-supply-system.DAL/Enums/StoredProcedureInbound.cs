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
        /// Получение всех лотов.
        /// </summary>
        [Description("Get_All_Lot_Items")]
        GetAllLotItems,
        /// <summary>
        /// Получение всех отправителей.
        /// </summary>
        [Description("Get_All_Shippers")]
        GetAllShippers,
        /// <summary>
        /// Получение всех перевозчиков.
        /// </summary>
        [Description("Get_All_Carriers")]
        GetAllCarriers,
        /// <summary>
        /// Получение всех перевозчиков.
        /// </summary>
        [Description("Get_All_Terms_Of_Delivery")]
        GetAllTermsOfDelivery,
        /// <summary>
        /// Получение всех типов транспорта.
        /// </summary>
        [Description("Get_All_Transport_Types")]
        GetAllTransportTypes,
        /// <summary>
        /// Получение всех локаций.
        /// </summary>
        [Description("Get_All_Locations")]
        GetAllLocations,
        /// <summary>
        /// Получение всех типов локаций.
        /// </summary>
        [Description("Get_All_Location_Types")]
        GetAllLocationTypes,
        /// <summary>
        /// Получение всего транспорта.
        /// </summary>
        [Description("Get_All_Transport_Items")]
        GetAllTransportItems,
        /// <summary>
        /// Получение всего транспорта.
        /// </summary>
        [Description("Add_New_Transport")]
        AddNewTransport,
        /// <summary>
        /// Получение всех инвойсов.
        /// </summary>
        [Description("Get_All_Invoice_Items")]
        GetAllInvoiceItems,
        /// <summary>
        /// Добавление нового инвойса.
        /// </summary>
        [Description("Add_New_Invoice")]
        AddNewInvoice,
        /// <summary>
        /// Удаление инвойса по уникальному идентификатору.
        /// </summary>
        [Description("Delete_Invoice")]
        DeleteInvoice,
        /// <summary>
        /// Добавление нового лота.
        /// </summary>
        [Description("Add_New_Lot")]
        AddNewLot,
        /// <summary>
        /// Получение всех типов контейнеров.
        /// </summary>
        [Description("Get_All_Container_Types")]
        GetAllContainerTypes,
        /// <summary>
        /// Получение всех контейнеров.
        /// </summary>
        [Description("Get_All_Containers")]
        GetAllContainers,
        /// <summary>
        /// Получение всех делатей.
        /// </summary>
        [Description("Get_All_Parts")]
        GetAllPartsInContainer,
        /// <summary>
        /// Добавление нового контейнера.
        /// </summary>
        [Description("Add_New_Container")]
        AddNewContainer,
        /// <summary>
        /// Получение всех кейсов.
        /// </summary>
        [Description("Get_All_Cases")]
        GetAllCases,
        /// <summary>
        /// Добавление нового кейса.
        /// </summary>
        [Description("Add_New_Case")]
        AddNewCase,
        /// <summary>
        /// Получение всех типов упаковок.
        /// </summary>
        [Description("Get_All_Packing_Types")]
        GetAllPackingTypes,
        /// <summary>
        /// Получение всех типов детали
        /// </summary>
        [Description("Get_All_Part_Types")]
        GetAllPartTypes,
        /// <summary>
        /// Получение всех деталей в инвойсе
        /// </summary>
        [Description("Get_All_Parts_In_Invoice")]
        GetAllPartsInInvoice,
        /// <summary>
        /// добавление новой детали в контейнер
        /// </summary>
        [Description("Add_New_Part_In_Container")]
        AddNewPartInContainer,
        /// <summary>
        /// добавление новой детали в инвойс
        /// </summary>
        [Description("Add_New_Part_In_Invoice")]
        AddNewPartInInvoice,
        /// <summary>
        /// получение трейсинга
        /// </summary>
        [Description("Get_All_Tracing")]
        GetAllTracing,
        /// <summary>
        /// добавление трейсинга
        /// </summary>
        [Description("Add_New_Trace")]
        AddNewTrace
    }
}
