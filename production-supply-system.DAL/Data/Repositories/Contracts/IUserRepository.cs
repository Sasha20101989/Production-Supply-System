using System.Threading.Tasks;

using DAL.Models;

namespace DAL.Data.Repositories.Contracts
{
    /// <summary>
    /// Репозиторий, отвечающий за доступ к пользовательской информации.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Получает информацию о пользователе.
        /// </summary>
        User GetCurrentUser();
        /// <summary>
        /// Получает информацию о пользователе асинхронно.
        /// </summary>
        /// <param name="userAccount">Учетная запись пользователя, для которой требуется получить информацию.</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую информацию о пользователе.</returns>
        Task<User> GetUserInfoAsync(string userAccount);
    }
}
