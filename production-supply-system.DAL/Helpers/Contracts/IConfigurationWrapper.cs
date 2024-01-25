namespace DAL.Helpers.Contracts
{
    /// <summary>
    /// Интерфейс для обертки доступа к конфигурационным данным приложения.
    /// </summary>
    public interface IConfigurationWrapper
    {
        /// <summary>
        /// Получает строку подключения из конфигурации по имени.
        /// </summary>
        /// <param name="name">Имя строки подключения.</param>
        string GetConnectionString(string name);
    }
}
