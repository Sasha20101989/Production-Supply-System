using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DAL.DataAccess.Contracts
{
    /// <summary>
    /// Интерфейс для выполнения асинхронных запросов и команд SQL
    /// </summary>
    public interface ISqlMapper
    {
        /// <summary>
        /// Выполняет асинхронный SQL-запрос и возвращает результат
        /// </summary>
        /// <typeparam name="T">Тип результата</typeparam>
        /// <param name="connection">Соединение с базой данных.</param>
        /// <param name="sql">Текст SQL-команды.</param>
        /// <param name="param">Параметры команды.</param>
        /// <param name="transaction">Транзакция.</param>
        /// <param name="commandTimeout">Таймаут выполнения команды.</param>
        /// <param name="commandType">Тип команды (текст, хранимая процедура и т. д.).</param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Выполняет асинхронную SQL-команду.
        /// </summary>
        /// <param name="connection">Соединение с базой данных.</param>
        /// <param name="sql">Текст SQL-команды.</param>
        /// <param name="param">Параметры команды.</param>
        /// <param name="transaction">Транзакция.</param>
        /// <param name="commandTimeout">Таймаут выполнения команды.</param>
        /// <param name="commandType">Тип команды (текст, хранимая процедура и т. д.).</param>
        Task ExecuteAsync(IDbConnection connection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }
}
