using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Data.Repositories.Contracts;
using DAL.Models.Contracts;

namespace DAL.Data.Repositories
{
    public class Repository<TModel> : IRepository<TModel> where TModel : IEntity
    {
        private readonly SqlServerData<TModel> _sqlServerData;

        public Repository(SqlServerData<TModel> sqlServerData)
        {
            _sqlServerData = sqlServerData;
        }

        public void RefreshData()
        {
            _sqlServerData.Refresh();
        }

        public async Task<TModel> CreateAsync(TModel entity, Enum storedProcedure, object parameters)
        {
            return await _sqlServerData.CreateAsync(entity, storedProcedure, parameters);
        }

        public async Task<IEnumerable<TModel>> GetAllAsync(bool beforeRefresh = false)
        {
            return await _sqlServerData.GetAllAsync(beforeRefresh);
        }

        public async Task<TModel> GetByIdAsync(int id)
        {
            return await _sqlServerData.GetByIdAsync(id);
        }

        public async Task RemoveAsync(int id, Enum storedProcedure)
        {
            await _sqlServerData.RemoveAsync(id, storedProcedure);
        }

        public async Task UpdateAsync(TModel entity, Enum storedProcedure, object parameters)
        {
            await _sqlServerData.UpdateAsync(entity, storedProcedure, parameters);
        }

        public async Task<bool> ExistsAsync(TModel entity)
        {
            return await GetByIdAsync(entity.Id) is not null;
        }

        public async Task<bool> TestConnectionAsync()
        {
            return await _sqlServerData.TestConnectionAsync();
        }
    }
}
