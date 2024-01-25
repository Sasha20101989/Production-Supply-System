using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Cache.Manager.WPF;
using DAL.Models;
using File.Manager;
using Microsoft.Extensions.Options;
using Moq;
using production_supply_system.TEST.MockData;
using UI_Interface.Contracts.Services;
using UI_Interface.Helpers;
using UI_Interface.Services;
using UI_Interface.ViewModels;
using Xunit;

namespace production_supply_system.TEST.UI.Services
{
    public class UserDataServiceTests
    {
        private static bool isAppInitialized = false;

        public UserDataServiceTests()
        {
            if (!isAppInitialized)
            {
                _ = new Application();
                isAppInitialized = true;
            }
        }

        [Fact]
        public void ImageFromString_ConvertsBase64StringToBitmapImage()
        {
            // Arrange

            string base64Data = "iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAIAAAD/gAIDAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyZpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTQ1IDc5LjE2MzQ5OSwgMjAxOC8wOC8xMy0xNjo0MDoyMiAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTkgKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOkYzNENGRUREMjBBNTExRTlCNTA0Q0EwODE4QjdBRUI3IiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOkYzNENGRURFMjBBNTExRTlCNTA0Q0EwODE4QjdBRUI3Ij4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6RjM0Q0ZFREIyMEE1MTFFOUI1MDRDQTA4MThCN0FFQjciIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6RjM0Q0ZFREMyMEE1MTFFOUI1MDRDQTA4MThCN0FFQjciLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7DwHPrAAAF/UlEQVR42uyc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";

            BitmapImage expectedImage = new();

            // Act

            BitmapImage result = ImageHelper.ImageFromString(base64Data);

            // Assert

            Assert.NotNull(result);

            _ = Assert.IsType<BitmapImage>(result);

            Assert.Equal(expectedImage.UriSource, result.UriSource);
        }

        [Fact]
        public void ImageFromAssetsFile_LoadsBitmapImageFromAssets()
        {
            // Arrange

            string fileName = "DefaultIcon.png";

            // Act

            BitmapImage result = ImageHelper.ImageFromAssetsFile(fileName);

            // Assert

            Assert.NotNull(result);

            _ = Assert.IsType<BitmapImage>(result);
        }

        [Fact]
        public void GetUser_WhenUserViewModelIsNull_ReturnsDefaultUserData()
        {
            // Arrange

            Mock<IFileManager> fileManagerMock = new();

            Mock<IIdentityService> identityServiceMock = new();

            AppConfig appConfig = new() { ConfigurationsFolder = "Production_Supply_System\\Configurations", UserFileName = "User.json" };

            UserDataService userDataService = new(fileManagerMock.Object, identityServiceMock.Object, Options.Create(appConfig));

            // Act

            UserViewModel result = userDataService.GetUser();

            // Assert

            Assert.NotNull(result);

            Assert.Equal(Environment.UserName, result.Name);

            Assert.NotNull(result.Photo);
        }

        [Fact]
        public void GetUserViewModelFromCache_ReturnsUserViewModelFromCacheData()
        {
            // Arrange

            Mock<IFileManager> fileManagerMock = new();

            Mock<IIdentityService> identityServiceMock = new();

            AppConfig appConfig = new() { ConfigurationsFolder = "Production_Supply_System\\Configurations", UserFileName = "User.json" };

            UserDataService userDataService = new(fileManagerMock.Object, identityServiceMock.Object, Options.Create(appConfig));

            User user = UserMocks.GetUserMock();

            UserViewModel userViewModel = new()
            {
                Account = user.Account,
                Name = user.Name,
                Patronymic = user.Patronymic
            };

            _ = fileManagerMock.Setup(
                manager => manager.Read<User>(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(UserMocks.GetUserMock());

            // Act

            MethodInfo methodInfo = typeof(UserDataService).GetMethod("GetUserViewModelFromCache", BindingFlags.NonPublic | BindingFlags.Instance);

            UserViewModel result = (UserViewModel)methodInfo.Invoke(userDataService, null);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(userViewModel.Name, result.Name);
        }
    }
}
