namespace production_supply_system.EntityFramework.DAL.Enums;
/// <summary>
/// Перечисление представляющее набор процессов приложения(отражение таблицы tbd_Processes)
/// </summary>
public enum AppProcess
{
    /// <summary>
    /// Загрузка инвойсов
    /// </summary>
    UploadInvoicesPartner2,
    /// <summary>
    /// Загрузка типов контейнеров
    /// </summary>
    UploadContainerTypesPartner2,
    /// <summary>
    /// Загрузка локальных VIN
    /// </summary>
    UploadLocalVinsPartner2,
    /// <summary>
    /// Экспорт трейсинга
    /// </summary>
    ExportFileToExcelPartner2
}