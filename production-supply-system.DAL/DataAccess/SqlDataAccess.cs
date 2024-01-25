using DAL.DbAccess.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DAL.Data.StoredProcedures;
using DAL.Helpers.Contracts;
using DAL.DataAccess.Contracts;
using System;

namespace DAL.DbAccess
{
    /// <summary>
    /// Реализация доступа к данным SQL.
    /// </summary>
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfigurationWrapper _configWrapper;
        private readonly ISqlMapper _sqlMapper;

        public SqlDataAccess(IConfigurationWrapper configWrapper, ISqlMapper sqlMapper)
        {
            _configWrapper = configWrapper;
            _sqlMapper = sqlMapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> LoadDataWithReturn<T>(
            Enum storedProcedure,
            object parameters = null,
            string connectionId = "Default")
        {
            string connectionString = _configWrapper.GetConnectionString(connectionId);
            using IDbConnection connection = new SqlConnection(connectionString);

            return await _sqlMapper.QueryAsync<T>(
                connection,
                StoredProcedures.Map[storedProcedure.GetType()].Invoke(storedProcedure),
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        /// <inheritdoc />
        public async Task SaveData(
            Enum storedProcedure,
            object parameters = null,
            string connectionId = "Default")
        {
            using IDbConnection connection = new SqlConnection(_configWrapper.GetConnectionString(connectionId));

            await _sqlMapper.ExecuteAsync(
                connection,
                StoredProcedures.Map[storedProcedure.GetType()].Invoke(storedProcedure),
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> LoadData<T>(
            Enum storedProcedure,
            object parameters = null,
            string connectionId = "Default"
        )
        {
            using IDbConnection connection = new SqlConnection(_configWrapper.GetConnectionString(connectionId));
            return await _sqlMapper.QueryAsync<T>(
                connection,
                StoredProcedures.Map[storedProcedure.GetType()].Invoke(storedProcedure),
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
