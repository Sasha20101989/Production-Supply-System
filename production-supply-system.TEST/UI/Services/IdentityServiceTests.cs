using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Documents;

using BLL.Contracts;
using BLL.Services;

using DAL.Models;

using Microsoft.Extensions.Logging;

using Moq;

using production_supply_system.TEST.MockData;

using UI_Interface.Helpers;
using UI_Interface.Services;

using Xunit;

namespace production_supply_system.TEST.UI.Services
{
    public class IdentityServiceTests
    {
        [Fact]
        public async Task LoginAsync_UserExists_FiresLoggedInEvent()
        {
            // Arrange

            Mock<IUserService> userServiceMock = new();

            _ = userServiceMock.Setup(service => service.GetUserInfoAsync(It.IsAny<string>()))
                           .ReturnsAsync(UserMocks.GetUserMock());

            IdentityService identityService = new(userServiceMock.Object, Mock.Of<ILogger<IdentityService>>());

            bool eventFired = false;

            identityService.LoggedIn += (sender, args) => eventFired = true;

            // Act

            LoginResultType result = await identityService.LoginAsync();

            // Assert

            Assert.Equal(LoginResultType.Success, result);

            Assert.True(eventFired);
        }

        [Fact]
        public async Task LoginAsync_UserDoesNotExist_ReturnsUnauthorized()
        {
            // Arrange

            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<IdentityService>> loggerMock = new();

            _ = userServiceMock.Setup(service => service.GetUserInfoAsync(It.IsAny<string>()))
                           .ReturnsAsync((User)null);

            IdentityService identityService = new(userServiceMock.Object, Mock.Of<ILogger<IdentityService>>());

            bool eventFired = false;

            identityService.LoggedIn += (sender, args) => eventFired = true;

            // Act

            LoginResultType result = await identityService.LoginAsync();

            // Assert

            Assert.Equal(LoginResultType.Unauthorized, result);

            Assert.False(eventFired);
        }

        [Fact]
        public void Logout_FiresLoggedOutEvent()
        {
            // Arrange

            IdentityService identityService = new(Mock.Of<IUserService>(), Mock.Of<ILogger<IdentityService>>());


            bool eventFired = false;

            identityService.LoggedOut += (sender, args) => eventFired = true;

            // Act

            identityService.Logout();

            // Assert

            Assert.True(eventFired);
        }

        [Fact]
        public void GetAccountUserName_ReturnsEnvironmentUserName()
        {
            // Arrange

            IdentityService identityService = new(Mock.Of<IUserService>(), Mock.Of<ILogger<IdentityService>>());

            // Act

            MethodInfo methodInfo = typeof(IdentityService).GetMethod("GetAccountUserName", BindingFlags.NonPublic | BindingFlags.Static);

            string result = (string)methodInfo.Invoke(identityService, null);

            // Assert

            Assert.Equal(Environment.UserName, result);
        }

        [Fact]
        public async Task IsUserExistsAsync_UserExists_ReturnsTrue()
        {
            // Arrange

            Mock<IUserService> userServiceMock = new();

            _ = userServiceMock.Setup(service => service.GetUserInfoAsync(It.IsAny<string>()))
                           .ReturnsAsync(UserMocks.GetUserMock());

            IdentityService identityService = new(userServiceMock.Object, Mock.Of<ILogger<IdentityService>>());

            // Act

            MethodInfo methodInfo = typeof(IdentityService).GetMethod("IsUserExistsAsync", BindingFlags.NonPublic | BindingFlags.Instance);

            bool result = await (Task<bool>)methodInfo.Invoke(identityService, null);

            // Assert

            Assert.True(result);
        }

        [Fact]
        public async Task IsUserExistsAsync_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange

            Mock<IUserService> userServiceMock = new();

            _ = userServiceMock.Setup(service => service.GetUserInfoAsync(It.IsAny<string>()))
                           .ReturnsAsync((User)null);

            IdentityService identityService = new(userServiceMock.Object, Mock.Of<ILogger<IdentityService>>());

            // Act
         
            MethodInfo methodInfo = typeof(IdentityService).GetMethod("IsUserExistsAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            
            bool result = await (Task<bool>)methodInfo.Invoke(identityService, null);
            // Assert

            Assert.False(result);
        }
    }
}