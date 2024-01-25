using System.Threading.Tasks;
using BLL.Contracts;
using DAL.Data.Repositories.Contracts;
using DAL.Models;

namespace BLL.Services
{
    /// <summary>
    /// Сервис, отвечающий за операции, связанные с пользователями.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserService"/>.
        /// </summary>
        /// <param name="userRepository">Репозиторий для доступа к информации о пользователях.</param>
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<User> GetUserInfoAsync(string userAccount)
        {
            return await _userRepository.GetUserInfoAsync(userAccount);
        }

        /// <inheritdoc />
        public User GetCurrentUser()
        {
            return _userRepository.GetCurrentUser();
        }
    }
}
