using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using DAL.DataAccess.Contracts;

using Dapper;

namespace DAL.DataAccess
{
    /// <summary>
    /// Реализация интерфейса ISqlMapper с использованием Dapper для выполнения асинхронных запросов и команд SQL.
    /// </summary>
    public class DapperSqlMapper : ISqlMapper
    {
        /// <inheritdoc/>
        public Task ExecuteAsync(IDbConnection connection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.ExecuteAsync(connection, sql, param, transaction, commandTimeout, commandType);

        }

        /// <inheritdoc/>
        public Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.QueryAsync<T>(connection, sql, param, transaction, commandTimeout, commandType);
        }
    }
}
