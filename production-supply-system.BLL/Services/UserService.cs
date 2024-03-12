using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Contracts;
using DAL.Data.Repositories.Contracts;
using DAL.Models;

using DocumentFormat.OpenXml.Spreadsheet;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace BLL.Services
{
    /// <summary>
    /// Сервис, отвечающий за операции, связанные с пользователями.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        private readonly IRepository<Section> _sectionRepository;

        private readonly ILogger<UserService> _logger;

        private User _user;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserService"/>.
        /// </summary>
        /// <param name="userRepository">Репозиторий для доступа к информации о пользователях.</param>
        public UserService(IRepository<User> userRepository, 
            IRepository<Section> sectionRepository,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;

            _sectionRepository = sectionRepository;

            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<User> GetUserInfoAsync(string userAccount)
        {
            IEnumerable<User> users;

            try
            {
                users = await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get all users list: {JsonConvert.SerializeObject(ex)}");

                throw;
            }

            _logger.LogInformation($"Get user by account from received list: {userAccount}");

            _user = users.SingleOrDefault(u => u.Account == userAccount);

            if (_user is null)
            {
                _logger.LogWarning($"User with account: {userAccount} not found.");

                return null;
            }

            try
            {
                if (_user.SectionId <= 0)
                {
                    throw new Exception("SectionId should be greater than 0.");
                }

                _user.Section = await _sectionRepository.GetByIdAsync(_user.SectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error get section by section id {_user.SectionId}: {JsonConvert.SerializeObject(ex)}");

                throw;
            }

            return _user;
        }

        /// <inheritdoc />
        public User GetCurrentUser()
        {
            return _user;
        }

        /// <inheritdoc />
        public async Task<bool> IsAccessAllowedAsync()
        {
            return await _userRepository.TestConnectionAsync();
        }
    }
}
