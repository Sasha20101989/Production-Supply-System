using DAL.Data.Contracts;
using DAL.Models;
using DAL.Repositories;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace production_supply_system.TEST.DAL.Data.Repositories
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task GetUserInfoAsync_ValidUserAccount_ReturnsUserInfo()
        {
            // Arrange

            string validUserAccount = "testUserAccount";

            User expectedUser = new() { Id = 1, Name = "TestUser" };

            Mock<IUserData> mockUserData = new();

            Mock<ISectionData> mockSectionData = new();

            _ = mockUserData.Setup(u => u.GetUserAsync(validUserAccount)).ReturnsAsync(expectedUser);

            UserRepository userRepository = new(mockUserData.Object, mockSectionData.Object);

            // Act

            User result = await userRepository.GetUserInfoAsync(validUserAccount);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async Task GetUserInfoAsync_InvalidUserAccount_ReturnsNull()
        {
            // Arrange

            string invalidUserAccount = "nonexistentUser";

            Mock<IUserData> mockUserData = new();

            Mock<ISectionData> mockSectionData = new();

            _ = mockUserData.Setup(u => u.GetUserAsync(invalidUserAccount)).ReturnsAsync((User)null);

            UserRepository userRepository = new(mockUserData.Object, mockSectionData.Object);

            // Act

            User result = await userRepository.GetUserInfoAsync(invalidUserAccount);

            // Assert

            Assert.Null(result);
        }
    }
}
