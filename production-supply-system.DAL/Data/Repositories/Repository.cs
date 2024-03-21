using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Data.Repositories.Contracts;
using DAL.Models.Contracts;

namespace DAL.Data.Repositories
{
    public class Repository<TModel>(SqlServerData<TModel> sqlServerData) : IRepository<TModel> where TModel : IEntity
    {
        public void RefreshData()
        {
            sqlServerData.Refresh();
        }

        public async Task<TModel> CreateAsync(TModel entity, Enum storedProcedure, object parameters)
        {
            return await sqlServerData.CreateAsync(entity, storedProcedure, parameters);
        }

        public async Task<IEnumerable<TModel>> GetAllAsync(bool beforeRefresh = false)
        {
            return await sqlServerData.GetAllAsync(beforeRefresh);
        }

        public async Task<TModel> GetByIdAsync(int id)
        {
            return await sqlServerData.GetByIdAsync(id);
        }

        public async Task RemoveAsync(int id, Enum storedProcedure)
        {
            await sqlServerData.RemoveAsync(id, storedProcedure);
        }

        public async Task UpdateAsync(Enum storedProcedure, object parameters)
        {
            await sqlServerData.UpdateAsync(storedProcedure, parameters);
        }

        public async Task<bool> ExistsAsync(TModel entity)
        {
            return await GetByIdAsync(entity.Id) is not null;
        }

        public async Task<bool> TestConnectionAsync()
        {
            return await sqlServerData.TestConnectionAsync();
        }
    }
}
