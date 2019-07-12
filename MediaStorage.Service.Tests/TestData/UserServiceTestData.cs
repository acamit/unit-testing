using MediaStorage.Common.ViewModels.User;
using MediaStorage.Data.Entities;
using System;

namespace MediaStorage.Service.Tests.TestData
{
    public static class UserServiceTestData
    {
        public static Guid Guid => new Guid();
        public static User User => new User() { Id = Guid, Username = "Amit Chawla", Mail = "demo@gmail.com" };
        public static UserPostViewModel newUser => new UserPostViewModel()
        {
            IsActive = true,
            Id = Guid,
            Username = "Amit Chawla",
            Mail = "demo@gmail.com"
        };
        public static LoginViewModel loginModel = new LoginViewModel() { Password = "password", Username = "UserName" };
    }
}
