using MediaStorage.Common;
using MediaStorage.Data.Entities;
using MediaStorage.Data.Interfaces.IRepository;
using MediaStorage.Data.Read;
using MediaStorage.Service.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using static MediaStorage.Common.Constants;
namespace MediaStorage.Service.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private UserService _userService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IUserReadRepository> _userReadRepository;
        private readonly Mock<IUserWriteRepository> _userWriteRepository;
        private readonly Mock<IMailSender> _mailSender;

        public UserServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _userReadRepository = new Mock<IUserReadRepository>();
            _userWriteRepository = new Mock<IUserWriteRepository>();
            _mailSender = new Mock<IMailSender>();
        }
        #region GetAllUsers
        [TestMethod]
        public void GetAllUsers_ShouldCallRepoToFetchAllUSers()
        {
            var users = _userService.GetAllUsers();
            _userReadRepository.Verify(x => x.GetAllUsers(), Times.Once, "Should Call Repository to fetch all users");
        }
        #endregion

        #region GetUserById
        [TestMethod]
        public void GetUserById_ShouldCallRepoToUser()
        {
            var user = _userService.GetUserById(UserServiceTestData.Guid);

            _userReadRepository.Verify(x => x.GetUserById(It.IsAny<Guid>()), Times.Once, "Should Call Repository to user Details");
        }

        [TestMethod]
        public void GetUserById_UserExist_ShouldReturnCorrectUser()
        {
            _userReadRepository.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(UserServiceTestData.User);

            var user = _userService.GetUserById(UserServiceTestData.Guid);

            Assert.IsNotNull(user);
            Assert.AreEqual(user.Id, UserServiceTestData.User.Id);
            Assert.AreEqual(user.Mail, UserServiceTestData.User.Mail);
            Assert.AreEqual(user.Username, UserServiceTestData.User.Username);
        }
        [TestMethod]
        public void GetUserById_UserExist_ShouldReturnNull()
        {
            _userReadRepository.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns((User)null);

            var user = _userService.GetUserById(UserServiceTestData.Guid);

            Assert.IsNull(user);
        }

        #endregion

        #region Login
        public void Login_ShouldCallRepoToUser()
        {
            var user = _userService.Login(UserServiceTestData.loginModel);
            _userReadRepository.Verify(x => x.GetByUserIdPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once, "Should Call Repository to user Details");
        }

        [TestMethod]
        public void Login_UserExist_ShouldReturnSuccessStatus()
        {
            _userReadRepository.Setup(x => x.GetByUserIdPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(UserServiceTestData.User);

            var result = _userService.Login(UserServiceTestData.loginModel);
            Assert.IsTrue(result.IsSuccessful, "Issuccessfull should be set to True");
            Assert.AreEqual(result.Message, Constants.LoginSuccessMessage, "Should set update department success message correctly.");

        }
        [TestMethod]
        public void Login_UserExist_ShouldReturnFailedStatus()
        {
            _userReadRepository.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(UserServiceTestData.User);

            var result = _userService.Login(UserServiceTestData.loginModel);
            Assert.IsFalse(result.IsSuccessful, "Issuccessfull should be set to False");
            Assert.AreEqual(result.Message, Constants.UserNotFoundMessage, "Should set update department failed message correctly.");

        }
        #endregion

        #region AddUser
        [TestMethod]
        public void AddUser_UserExist_ShouldVerifyIfUserExistsByUserName()
        {
            var result = _userService.AddUser(UserServiceTestData.newUser);
            _userReadRepository.Verify(x => x.GetByUserName(It.IsAny<string>()), Times.Once, "Should Verify for duplicate username");
        }
        [TestMethod]
        public void AddUser_UserExist_ShouldSetFailedMessageToUserExists()
        {
            _userReadRepository.Setup(x => x.GetByUserName(It.IsAny<string>())).Returns(UserServiceTestData.User);
            var result = _userService.AddUser(UserServiceTestData.newUser);
            Assert.IsFalse(result.IsSuccessful, "Should Return failed status");
            Assert.AreEqual(result.Message, UserNameAlreadyExistsMessage, "Should set username already exists message");
        }

        [TestMethod]
        public void AddUser_UserExist_ShouldVerifyIfUserExistsByMailId()
        {
            var result = _userService.AddUser(UserServiceTestData.newUser);
            _userReadRepository.Verify(x => x.GetByUserByEmail(It.IsAny<string>()), Times.Once, "Should Verify for duplicate username");
        }
        [TestMethod]
        public void AddUser_UserExist_ShouldSetFailedMessageToMailExists()
        {
            _userReadRepository.Setup(x => x.GetByUserByEmail(It.IsAny<string>())).Returns(UserServiceTestData.User);
            var result = _userService.AddUser(UserServiceTestData.newUser);
            Assert.IsFalse(result.IsSuccessful, "Should Return failed status");
            Assert.AreEqual(result.Message, MailAlReadyExistsMessage, "Should set mail already exists message");
        }
        [TestMethod]
        public void AddUser_ValidUser_ShouldCallAddUserOnRepsitory()
        {
            var result = _userService.AddUser(UserServiceTestData.newUser);
            _userWriteRepository.Verify(x => x.AddUser(It.IsAny<User>()), Times.Once, "Should call Add user on repository");
        }

        [TestMethod]
        public void AddUser_ValidUser_ShouldSendUserAddedEmail()
        {
            var result = _userService.AddUser(UserServiceTestData.newUser);
            _mailSender.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once, "Should send user added email");
        }

        [TestMethod]
        public void AddUser_UserAdded_ShouldSetSuccessMessage()
        {
            _userWriteRepository.Setup(x => x.AddUser(It.IsAny<User>())).Returns(true);
            var result = _userService.AddUser(UserServiceTestData.newUser);
            Assert.IsTrue(result.IsSuccessful, "Should Return success status");
            Assert.AreEqual(result.Message, AddUserSuccessMessage, "Should set user added message");
        }

        [TestMethod]
        public void AddUser_UserAdded_ShouldSetFailedMessage()
        {
            _userWriteRepository.Setup(x => x.AddUser(It.IsAny<User>())).Returns(false);
            var result = _userService.AddUser(UserServiceTestData.newUser);
            Assert.IsFalse(result.IsSuccessful, "Should return failed status");
            Assert.AreEqual(result.Message, AddUserFailedMessage, "Should set add user failed message");
        }
        #endregion

        #region RemoveUser
        [TestMethod]
        public void RemoveUser_ShouldGetUserByGuid()
        {
            var result = _userService.RemoveUser(UserServiceTestData.Guid);
            _userReadRepository.Verify(x => x.GetUserById(It.IsAny<Guid>()), Times.Once, "Should check if user exists before deleting");
        }
        [TestMethod]
        public void RemoveUser_UserExists_ShouldCallDeleteMethod()
        {
            _userReadRepository.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(UserServiceTestData.User);

            var result = _userService.RemoveUser(UserServiceTestData.Guid);

            _userWriteRepository.Verify(x => x.DeleteUser(It.IsAny<User>()), Times.Once, "Should call repository method for delete user");
        }
        [TestMethod]
        public void RemoveUser_UserExistsDeleteSuccess_ShouldSetSuccessMessage()
        {
            _userReadRepository.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(UserServiceTestData.User);
            _userWriteRepository.Setup(x => x.DeleteUser(It.IsAny<User>())).Returns(true);

            var result = _userService.RemoveUser(UserServiceTestData.Guid);

            Assert.IsTrue(result.IsSuccessful, "Should Set Succes status");
            Assert.AreEqual(result.Message, DeleteUserSuccessMessage);
        }
        [TestMethod]
        public void RemoveUser_UserExistsDeleteFailed_ShouldSetFailedMessage()
        {
            _userReadRepository.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(UserServiceTestData.User);
            _userWriteRepository.Setup(x => x.DeleteUser(It.IsAny<User>())).Returns(false);

            var result = _userService.RemoveUser(UserServiceTestData.Guid);

            Assert.IsFalse(result.IsSuccessful, "Should Set failed status");
            Assert.AreEqual(result.Message, DeleteUserFailedMessage);
        }
        [TestMethod]
        public void RemoveUser_UserDoesNotExists_ShouldSetFailedMessage()
        {
            _userReadRepository.Setup(x => x.GetUserById(It.IsAny<Guid>())).Returns(UserServiceTestData.User);
            _userWriteRepository.Setup(x => x.DeleteUser(It.IsAny<User>())).Returns(false);

            var result = _userService.RemoveUser(UserServiceTestData.Guid);

            Assert.IsFalse(result.IsSuccessful, "Should Set failed status");
            Assert.AreEqual(result.Message, DeleteUserFailedMessage);
        }
        #endregion
        [TestInitialize]
        public void GetServiceInstance()
        {
            _userService = new UserService(_userRepository.Object, _mailSender.Object);
            _userRepository.Setup(x => x.UserReadRepository).Returns(_userReadRepository.Object);
            _userRepository.Setup(x => x.UserWriteRepository).Returns(_userWriteRepository.Object);
        }
    }
}
