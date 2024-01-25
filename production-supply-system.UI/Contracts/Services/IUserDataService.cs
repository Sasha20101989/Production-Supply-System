using System;

using UI_Interface.ViewModels;

namespace UI_Interface.Contracts.Services
{
    /// <summary>
    /// Интерфейс службы данных пользователя, предоставляющий методы для управления
    /// и получения информации о пользователе, а также событие при обновлении данных пользователя.
    /// </summary>
    public interface IUserDataService
    {
        /// <summary>
        /// Событие, возникающее при обновлении данных пользователя.
        /// </summary>
        event EventHandler<UserViewModel> UserDataUpdated;

        /// <summary>
        /// Инициализирует службу данных пользователя.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Получает информацию о текущем пользователе.
        /// </summary>
        /// <returns>Модель представления данных о пользователе.</returns>
        UserViewModel GetUser();
    }
}
