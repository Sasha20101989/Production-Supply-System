namespace DAL.Enums
{
    /// <summary>
    /// Перечисление представляющее шаги процесса "AppProcess"(отражение поля Step_Name в таблице tbd_Processes_Steps)
    /// </summary>
    public enum Steps
    {
        /// <summary>
        /// Загрузка контейнеров без типов контейнеров и товаров и цен
        /// </summary>
        UploadLotContent,
        /// <summary>
        /// Загрузка типов контейнеров
        /// </summary>
        UploadContainerTypes,
        /// <summary>
        /// Загрузка цен товаров
        /// </summary>
        UploadPrice,
        /// <summary>
        /// 
        /// </summary>
        PPP
    }
}
