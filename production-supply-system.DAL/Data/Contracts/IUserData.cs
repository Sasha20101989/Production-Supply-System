using DAL.Models;
using System.Threading.Tasks;

namespace DAL.Data.Contracts
{
    /// <summary>
    /// Интерфейс данных для операций, связанных с пользователями.
    /// </summary>
    public interface IUserData
    {
        /// <summary>
        /// Асинхронно получает информацию о пользователе.
        /// </summary>
        /// <param name="userAccount">Учетная запись пользователя для получения информации.</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую информацию о пользователе.</returns>
        Task<User> GetUserAsync(string userAccount);
    }
}
