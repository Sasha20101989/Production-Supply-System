using DAL.Helpers.Contracts;

using Microsoft.Extensions.Configuration;

namespace DAL.Helpers
{
    /// <summary>
    /// Реализация интерфейса IConfigurationWrapper для обертки доступа к конфигурационным данным приложения.
    /// </summary>
    /// <remarks>
    /// Инициализирует новый экземпляр класса ConfigurationWrapper.
    /// </remarks>
    /// <param name="configuration">Интерфейс IConfiguration.</param>
    public class ConfigurationWrapper(IConfiguration configuration) : IConfigurationWrapper
    {

        /// <inheritdoc/>
        public string GetConnectionString(string name)
        {
            return configuration.GetConnectionString(name);
        }
    }
}
