using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DAL.DbAccess.Contracts;
using DAL.Models.Contracts;

using Dapper;

namespace DAL.Data
{
    public abstract class SqlServerData<T> where T : IEntity
    {
        private readonly ISqlDataAccess _db;

        protected internal Lazy<Task<IEnumerable<T>>> _dataItems;

        private readonly Enum _initialStoredProcedure;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="TermsOfDeliverySqlServerData"/> class.
        /// </summary>
        /// <param name="db">Служба доступа к данным SQL.</param>
        /// <param name="initialDataStoredProcedure">Хранимая процедура инициализирующая данные.</param>
        protected SqlServerData(ISqlDataAccess db, Enum initialDataStoredProcedure)
        {
            _db = db;

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
            IEnumerable<T> result = await _db.LoadDataAsync<T>(storedProcedure);

            return result;
        }

        public async Task UpdateAsync(Enum storedProcedure, object parameters)
        {
            await _db.SaveDataAsync(storedProcedure, parameters);

            Refresh();
        }

        public async Task RemoveAsync(int id, Enum storedProcedure)
        {
            DynamicParameters parameters = new();
            parameters.Add("@Id", id);

            await _db.SaveDataAsync(storedProcedure, parameters);

            Refresh();
        }

        public async Task<T> CreateAsync(T entity, Enum storedProcedure, object parameters)
        {
            IEnumerable<T> result = await _db.LoadDataAsync<T>(storedProcedure, parameters);

            T newColumn = result.First();

            entity.Id = newColumn.Id;


            Refresh();

            return entity;
        }
    }
}