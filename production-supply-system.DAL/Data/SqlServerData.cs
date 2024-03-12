using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DAL.DbAccess.Contracts;
using DAL.Models.Contracts;

using Dapper;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace DAL.Data
{
    public abstract class SqlServerData<T> where T : IEntity
    {
        private readonly ISqlDataAccess _db;

        private readonly ILogger<SqlServerData<T>> _logger;

        protected internal Lazy<Task<IEnumerable<T>>> _dataItems;

        private readonly Enum _initialStoredProcedure;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="TermsOfDeliverySqlServerData"/> class.
        /// </summary>
        /// <param name="db">Служба доступа к данным SQL.</param>
        /// <param name="initialDataStoredProcedure">Хранимая процедура инициализирующая данные.</param>
        protected SqlServerData(ISqlDataAccess db, ILogger<SqlServerData<T>> logger, Enum initialDataStoredProcedure)
        {
            _db = db;

            _logger = logger;

            _initialStoredProcedure = initialDataStoredProcedure;

            Refresh();
        }

        public void Refresh()
        {
            _dataItems = new Lazy<Task<IEnumerable<T>>>(() => GetAllDataItemsAsync(_initialStoredProcedure));
        }

        public async Task<bool> TestConnectionAsync()
        {
            return await _db.TestConnectionAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool beforeRefresh = false)
        {
            if (beforeRefresh)
            {
                Refresh();
            }

            return await _dataItems.Value;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            IEnumerable<T> dataItems = await _dataItems.Value;

            return dataItems.FirstOrDefault(c => c.Id == id);
        }

        private async Task<IEnumerable<T>> GetAllDataItemsAsync(Enum storedProcedure)
        {
            _logger.LogInformation($"Get all {typeof(T).Name} items");

            IEnumerable<T> result = await _db.LoadDataAsync<T>(storedProcedure);

            _logger.LogInformation($"List of {typeof(T).Name} items received");

            return result;
        }

        public async Task UpdateAsync(T entity, Enum storedProcedure, object parameters)
        {
            _logger.LogInformation($"Update {typeof(T).Name} with id {entity.Id} " +
                $"-> Updated information: {JsonConvert.SerializeObject(entity)}");

            await _db.SaveDataAsync(storedProcedure, parameters);

            _logger.LogInformation($"{typeof(T).Name} with id {entity.Id} updated");

            Refresh();
        }

        public async Task RemoveAsync(int id, Enum storedProcedure)
        {
            DynamicParameters parameters = new();
            parameters.Add("@Id", id);

            _logger.LogInformation($"Delete {typeof(T).Name} with id {id}");

            await _db.SaveDataAsync(storedProcedure, parameters);

            _logger.LogInformation($"{typeof(T).Name} with id {id} deleted");

            Refresh();
        }

        public async Task<T> CreateAsync(T entity, Enum storedProcedure, object parameters)
        {
            _logger.LogInformation($"Create {nameof(entity)}: {JsonConvert.SerializeObject(entity)}");

            IEnumerable<T> result = await _db.LoadDataAsync<T>(storedProcedure, parameters);

            T newColumn = result.First();

            entity.Id = newColumn.Id;

            _logger.LogInformation($"{nameof(entity)} with id {entity.Id} created");

            Refresh();

            return entity;
        }
    }
}