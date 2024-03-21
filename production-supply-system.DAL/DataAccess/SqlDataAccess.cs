using DAL.DbAccess.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DAL.Helpers.Contracts;
using DAL.DataAccess.Contracts;
using System;
using Dapper;
using DAL.Extensions;

namespace DAL.DbAccess
{
    /// <summary>
    /// Реализация доступа к данным SQL.
    /// </summary>
    public class SqlDataAccess(IConfigurationWrapper configWrapper, ISqlMapper sqlMapper) : ISqlDataAccess
    {

        /// <inheritdoc />
        public async Task<IEnumerable<T>> LoadDataWithReturnAsync<T>(
            Enum storedProcedure,
            object parameters = null,
            string connectionId = "Default")
        {
            string connectionString = configWrapper.GetConnectionString(connectionId);
            using SqlConnection connection = new(connectionString);

            return await sqlMapper.QueryAsync<T>(
                connection,
                StoredProceduresExtensions.Map[storedProcedure.GetType()].Invoke(storedProcedure),
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        /// <inheritdoc />
        public async Task SaveDataAsync(
            Enum storedProcedure,
            object parameters = null,
            string connectionId = "Default")
        {
            using SqlConnection connection = new(configWrapper.GetConnectionString(connectionId));

            await sqlMapper.ExecuteAsync(
                connection,
                StoredProceduresExtensions.Map[storedProcedure.GetType()].Invoke(storedProcedure),
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> LoadDataAsync<T>(
            Enum storedProcedure,
            object parameters = null,
            string connectionId = "Default"
        )
        {
            using SqlConnection connection = new(configWrapper.GetConnectionString(connectionId));
            return await sqlMapper.QueryAsync<T>(
                connection,
                StoredProceduresExtensions.Map[storedProcedure.GetType()].Invoke(storedProcedure),
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> TestConnectionAsync(string connectionId = "Default")
        {
            try
            {
                using SqlConnection connection = new(configWrapper.GetConnectionString(connectionId));

                await connection.OpenAsync();

                const string sqlQuery = "SELECT 1";

                int result = await connection.QueryFirstOrDefaultAsync<int>(sqlQuery);

                return result == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

