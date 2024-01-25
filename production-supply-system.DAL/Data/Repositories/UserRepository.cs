using System.Threading.Tasks;
using DAL.Data.Contracts;
using DAL.Data.Repositories.Contracts;
using DAL.Models;

namespace DAL.Repositories
{
    /// <summary>
    /// Реализация пользовательского репозитория для извлечения данных.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IUserData _userData;

        private readonly ISectionData _sectionData;

        private User _user;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="userData">Источник данных для информации о пользователе.</param>
        public UserRepository(IUserData userData, ISectionData sectionData)
        {
            _userData = userData;

            _sectionData = sectionData;
        }

        /// <inheritdoc />
        

        /// <inheritdoc />
        public User GetCurrentUser()
        {
            return _user;
        }

        /// <inheritdoc />
        public async Task<User> GetUserInfoAsync(string userAccount)
        {
            _user = await _userData.GetUserAsync(userAccount);

            if (_user is null)
            {
                return null;
            }

            _user.Section = await _sectionData.GetSectionByIdAsync(_user.SectionId);

            return _user;
        }
    }
}
