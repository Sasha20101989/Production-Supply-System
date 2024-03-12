using System.Threading.Tasks;
using DAL.Models;

namespace BLL.Contracts
{
    /// <summary>
    /// Интерфейс сервиса для операций, связанных с пользователями.
    /// </summary>
    public interface IUserService 
    {
        /// <summary>
        /// Получает информацию о пользователе.
        /// </summary>
        User GetCurrentUser();
        /// <summary>
        /// Асинхронно получает информацию о пользователе.
        /// </summary>
        /// <param name="userAccount">Учетная запись пользователя для получения информации.</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую информацию о пользователе.</returns>
        Task<User> GetUserInfoAsync(string userAccount);

        /// <summary>
        /// Тестирование соединения с базой данных
        /// </summary>
        /// <returns>Если соединение присутствует возвращает true, в противном случае false</returns>
        Task<bool> IsAccessAllowedAsync();
    }
}
