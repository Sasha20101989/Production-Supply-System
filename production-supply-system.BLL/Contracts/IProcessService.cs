using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Enums;
using DAL.Models.Master;

namespace BLL.Contracts
{
    /// <summary>
    /// Интерфейс сервиса для операций, связанных с информацией о процессе приложения.
    /// </summary>
    public interface IProcessService
    {
        /// <summary>
        /// Получает шагами для процесса приложения и по уникальному индентификатору секции пользователя
        /// </summary>
        /// <param name="sectionId">уникальный индентификатор секции пользователя</param>
        /// <param name="appProcess">индентификатор процесса прилодения</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая шаги процесса по уникальному секции и процессу приложения</returns>
        Task<IEnumerable<ProcessStep>> GetProcessStepsByUserSectionAsync(int sectionId, AppProcess appProcess);
    }
}
