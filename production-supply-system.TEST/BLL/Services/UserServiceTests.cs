using System.Threading.Tasks;

using BLL.Services;

using DAL.Data.Repositories.Contracts;
using DAL.Models;
using DAL.Models.Document;

using Moq;

using production_supply_system.TEST.MockData;

using Xunit;

namespace production_supply_system.TEST.BLL.Services
{
    public class UserServiceTests
    {
        //[Fact]
        //public async Task GetUserInfoAsync_ValidUserAccount_ReturnsUserInfo()
        //{
        //    // Arrange

        //    Mock<IRepository<User>> mockUserRepository = new();
        //    Mock<IRepository<Section>> mockSectionRepository = new();

        //    UserService userService = new(mockUserRepository.Object, mockSectionRepository.Object);

        //    string userAccount = "validUserAccount";

        //    User expectedUserInfo = UserMocks.GetUserMock();

        //    _ = mockUserRepository.Setup(repo => repo.GetUserInfoAsync(userAccount))
        //                      .ReturnsAsync(expectedUserInfo);

        //    // Act

        //    User result = await userService.GetUserInfoAsync(userAccount);

        //    // Assert

        //    Assert.NotNull(result);

        //    Assert.Equal(expectedUserInfo, result);
        //}

        //[Fact]
        //public async Task GetUserInfoAsync_InvalidUserAccount_ReturnsNull()
        //{
        //    // Arrange

        //    Mock<IUserRepository> mockUserRepository = new();

        //    UserService userService = new(mockUserRepository.Object);

        //    string invalidUserAccount = "invalidUserAccount";

        //    _ = mockUserRepository.Setup(repo => repo.GetUserInfoAsync(invalidUserAccount))
        //                      .ReturnsAsync((User)null);

        //    // Act

        //    User result = await userService.GetUserInfoAsync(invalidUserAccount);

        //    // Assert

        //    Assert.Null(result);
        //}
    }
}