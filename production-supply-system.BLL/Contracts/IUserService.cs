using System.Threading.Tasks;

using production_supply_system.EntityFramework.DAL.Models.UsersSchema;

namespace BLL.Contracts
{
    /// <summary>
    /// Интерфейс сервиса для операций, связанных с пользователями.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Асинхронно получает информацию о пользователе.
        /// </summary>
        /// <param name="userAccount">Учетная запись пользователя для получения информации.</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающую информацию о пользователе.</returns>
        Task<User> GetCurrentUser(string userAccount);
    }
}
