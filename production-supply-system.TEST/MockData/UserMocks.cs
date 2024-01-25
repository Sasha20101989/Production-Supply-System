using DAL.Models;

namespace production_supply_system.TEST.MockData
{
    public static class UserMocks
    {
        public static User GetUserMock()
        {
            User mockUser = new()
            {
                Id= 1,
                Name = "Test Name",
                Surname = "Test Surname",
                Patronymic = "Test Patronymic",
                Account = "Test Account",
                Department = "Test Department",
                Mail = "Test Mail",
                SectionId = 1,
                Section = new()
                {
                    SectionId = 1,
                    SectionName = "Test_Section"
                }
        };


            return mockUser;
        }
    }
}
