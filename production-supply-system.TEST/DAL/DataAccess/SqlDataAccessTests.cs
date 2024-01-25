using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using DAL.DataAccess.Contracts;
using DAL.DbAccess;
using DAL.Enums;
using DAL.Helpers.Contracts;
using DAL.Models;

using Moq;

using production_supply_system.TEST.MockData;

using Xunit;

namespace production_supply_system.TEST.DAL.DataAccess
{
    public class SqlDataAccessTests
    {
        private readonly Mock<IConfigurationWrapper> _configWrapperMock;

        private readonly Mock<ISqlMapper> _sqlMapperMock;

        public SqlDataAccessTests()
        {
            _configWrapperMock = new Mock<IConfigurationWrapper>();
            _sqlMapperMock = new Mock<ISqlMapper>();

            _ = _configWrapperMock.Setup(c => c.GetConnectionString(It.IsAny<string>())).Returns("Data Source=RU-NMGR-S0053;Initial Catalog=PSS;Integrated Security=True;Encrypt=False");
        }

        [Fact]
        public async Task LoadDataWithReturn_ValidParameters_CallsQueryAsync()
        {
            // Arrange

            SqlDataAccess sqlDataAccess = new(_configWrapperMock.Object, _sqlMapperMock.Object);

            StoredProcedureUsers storedProcedure = StoredProcedureUsers.GetUserByAccount;

            User parameters = UserMocks.GetUserMock();

            List<User> queryResult = new() { UserMocks.GetUserMock() };

            _sqlMapperMock.Setup(c => c.QueryAsync<User>(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                          .ReturnsAsync(queryResult)
                          .Verifiable();

            // Act

            IEnumerable<User> result = await sqlDataAccess.LoadDataWithReturn<User>(storedProcedure, parameters, "Default");

            // Assert

            Assert.NotNull(result);

            Assert.Equal(queryResult, result);
        }

        [Fact]
        public async Task SaveData_ValidParameters_CallsExecuteAsync()
        {
            // Arrange

            SqlDataAccess sqlDataAccess = new(_configWrapperMock.Object, _sqlMapperMock.Object);

            StoredProcedureUsers storedProcedure = StoredProcedureUsers.GetUserByAccount;

            User parameters = UserMocks.GetUserMock();

            _sqlMapperMock.Setup(c => c.ExecuteAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                          .Verifiable();

            // Act

            await sqlDataAccess.SaveData(storedProcedure, parameters, "Default");
        }

        [Fact]
        public async Task LoadData_ValidParameters_CallsQueryAsync()
        {
            // Arrange

            SqlDataAccess sqlDataAccess = new(_configWrapperMock.Object, _sqlMapperMock.Object);

            StoredProcedureUsers storedProcedure = StoredProcedureUsers.GetUserByAccount;

            object parameters = new { IsActive = true };

            List<User> queryResult = new() { UserMocks.GetUserMock() };

            _sqlMapperMock.Setup(c => c.QueryAsync<User>(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                          .ReturnsAsync(queryResult)
                          .Verifiable();

            // Act

            IEnumerable<User> result = await sqlDataAccess.LoadData<User>(storedProcedure, parameters, "Default");

            // Assert

            Assert.NotNull(result);

            Assert.Equal(queryResult, result);
        }
    }
}
