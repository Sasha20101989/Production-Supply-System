using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models.Contracts;

namespace DAL.Data.Repositories.Contracts
{
    public interface IRepository<TModel> where TModel : IEntity
    {
        Task<IEnumerable<TModel>> GetAllAsync(bool beforeRefresh = false);

        Task<TModel> CreateAsync(TModel entity, Enum storedProcedure, object parameters);

        Task<TModel> GetByIdAsync(int id);

        Task RemoveAsync(int id, Enum storedProcedure);

        Task UpdateAsync(TModel entity, Enum storedProcedure, object parameters);

        Task<bool> ExistsAsync(TModel entity);

        void RefreshData();

        Task<bool> TestConnectionAsync();
    }
}
