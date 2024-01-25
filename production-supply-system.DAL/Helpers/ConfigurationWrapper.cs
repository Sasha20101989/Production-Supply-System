using DAL.Helpers.Contracts;

using Microsoft.Extensions.Configuration;

namespace DAL.Helpers
{
    /// <summary>
    /// Реализация интерфейса IConfigurationWrapper для обертки доступа к конфигурационным данным приложения.
    /// </summary>
    public class ConfigurationWrapper : IConfigurationWrapper
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Инициализирует новый экземпляр класса ConfigurationWrapper.
        /// </summary>
        /// <param name="configuration">Интерфейс IConfiguration.</param>
        public ConfigurationWrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public string GetConnectionString(string name)
        {
            return _configuration.GetConnectionString(name);
        }
    }
}
