using System;
using System.Threading.Tasks;

using production_supply_system.EntityFramework.DAL.Models.UsersSchema;

using UI_Interface.Helpers;

namespace UI_Interface.Contracts.Services
{
    /// <summary>
    /// Интерфейс службы идентификации, предоставляющий методы для входа и выхода пользователя.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Событие, возникающее при успешном входе пользователя.
        /// </summary>
        event EventHandler<User> LoggedIn;

        /// <summary>
        /// Событие, возникающее при выходе пользователя.
        /// </summary>
        event EventHandler LoggedOut;

        /// <summary>
        /// Асинхронно выполняет вход пользователя.
        /// </summary>
        /// <returns>Тип результата входа (успешный, отменен, неудачный, не определённая ошибка и т.д.).</returns>
        Task<LoginResultType> LoginAsync();

        /// <summary>
        /// Выполняет выход пользователя.
        /// </summary>
        void Logout();
    }
}
