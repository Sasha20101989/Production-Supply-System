using System.ComponentModel;

namespace DAL.Enums
{
    /// <summary>
    /// Перечисление представляющее набор типов локаций(отражение таблицы tbd_Types_Of_Location)
    /// </summary>
    public enum LocationType
    {
        [Description("Departure terminal")]
        DepartureTerminal,
        [Description("Transshipment port")]
        TransshipmentPort,
        [Description("Arrival terminal")]
        ArrivalTerminal,
        [Description("Customs terminal")]
        CustomsTerminal,
        [Description("Container yard")]
        ContainerYard,
        [Description("Final location")]
        FinalLocation,
    }
}
