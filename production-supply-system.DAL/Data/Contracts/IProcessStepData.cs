using System.Collections.Generic;
using System.Threading.Tasks;

using DAL.Enums;
using DAL.Models.Master;

namespace DAL.Data.Contracts
{
    /// <summary>
    /// Интерфейс данных для операций, связанных с шагами по процессу.
    /// </summary>
    public interface IProcessStepData
    {
        /// <summary>
        /// Сбрасывает предыдущее состояние обьекта и загружает из источника
        /// </summary>
        Task RefreshAsync();

        /// <summary>
        /// Получает шагами для процесса приложения и по уникальному индентификатору секции пользователя
        /// </summary>
        /// <param name="sectionId">уникальный индентификатор секции пользователя</param>
        /// <param name="appProcess">индентификатор процесса прилодения</param>
        /// <returns>Задача, представляющая асинхронную операцию, возвращающая шаги процесса по уникальному секции и процессу приложения</returns>
        Task<IEnumerable<ProcessStep>> GetProcessStepsByUserSectionAsync(int sectionId, AppProcess appProcess);
    }
}
