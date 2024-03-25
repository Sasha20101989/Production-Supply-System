using System.Collections.Generic;
using System.Threading.Tasks;

using production_supply_system.EntityFramework.DAL.Enums;
using production_supply_system.EntityFramework.DAL.MasterProcessContext.Models;

namespace BLL.Contracts
{
    /// <summary>
    /// Интерфейс сервиса для операций, связанных с информацией о процессе приложения.
    /// </summary>
    public interface IProcessService
    {
        /// <summary>
        /// Получает шаги для процесса приложения и по уникальному индентификатору секции пользователя
        /// </summary>
        /// <param name="sectionId">уникальный индентификатор секции пользователя</param>
        /// <param name="appProcess">индентификатор процесса прилодения</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая шаги процесса по уникальному секции и процессу приложения</returns>
        Task<IEnumerable<ProcessesStep>> GetProcessStepsByUserSectionAsync(AppProcess appProcess);
    }
}
