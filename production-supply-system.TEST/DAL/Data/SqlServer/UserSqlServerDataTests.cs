using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DAL.Data.SqlServer;
using DAL.DbAccess.Contracts;
using DAL.Enums;
using DAL.Models;

using Microsoft.Extensions.Logging;

using Moq;

using production_supply_system.TEST.MockData;

using Xunit;

namespace production_supply_system.TEST.DAL.Data.SqlServer {
    public class UserSqlServerDataTests
    {
        [Fact]
        public async Task GetUser_ValidAccount_ReturnsUser()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<UserSqlServerData>> mockLogger = new();

            UserSqlServerData userSqlServerData = new(mockDb.Object, mockLogger.Object);

            string userAccount = "validAccount";

            User expectedUser = UserMocks.GetUserMock();

            List<User> mockUserData = new() { expectedUser };

            _ = mockDb.Setup(db => db.LoadData<User>(StoredProcedureUsers.GetUserByAccount, It.IsAny<object>(), "Default"))
                .ReturnsAsync(mockUserData);

            // Act

            User result = await userSqlServerData.GetUserAsync(userAccount);

            // Assert

            Assert.NotNull(result);

            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async Task GetUser_InvalidAccount_ThrowsException()
        {
            // Arrange

            Mock<ISqlDataAccess> mockDb = new();

            Mock<ILogger<UserSqlServerData>> mockLogger = new();

            UserSqlServerData userSqlServerData = new(mockDb.Object, mockLogger.Object);

            string userAccount = "invalidAccount";

            _ = mockDb.Setup(db => db.LoadData<User>(StoredProcedureUsers.GetUserByAccount, It.IsAny<object>(), "Default"))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert

            _ = await Assert.ThrowsAsync<Exception>(() => userSqlServerData.GetUserAsync(userAccount));
        }
    }
}
