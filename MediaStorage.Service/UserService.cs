using MediaStorage.Common;
using MediaStorage.Common.ViewModels.User;
using MediaStorage.Data.Entities;
using MediaStorage.Data.Interfaces.IRepository;
using System;
using System.Collections.Generic;
using static MediaStorage.Common.Constants;
namespace MediaStorage.Service
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IMailSender _mailSender;
        public UserService(IUserRepository userRepository, IMailSender mailSender)
        {
            _userRepository = userRepository;
            _mailSender = mailSender;
        }
        public List<UserViewModel> GetAllUsers()
        {
            return _userRepository.UserReadRepository.GetAllUsers();
        }

        public UserPostViewModel GetUserById(Guid id)
        {
            User user = _userRepository.UserReadRepository.GetUserById(id);
            return user == null ? null : new UserPostViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Mail = user.Mail
            };
        }

        public ServiceResult Login(LoginViewModel model)
        {
            ServiceResult result = new ServiceResult();
            var user = _userRepository.UserReadRepository.GetByUserIdPassword(model.Username, model.Password);
            if (user == null)
            {
                result.SetFailure(UserNotFoundMessage);
            }
            else
            {
                result.SetSuccess(LoginSuccessMessage);
            }

            return result;
        }

        public ServiceResult AddUser(UserPostViewModel entity)
        {
            bool isUserAdded = false;
            var password = CreateRandomPassword();

            if (_userRepository.UserReadRepository.GetByUserName(entity.Username) != null)
            {
                return new ServiceResult(false, UserNameAlreadyExistsMessage);
            }

            if (_userRepository.UserReadRepository.GetByUserByEmail(entity.Mail) != null)
            {
                return new ServiceResult(false, MailAlReadyExistsMessage);
            }

            isUserAdded = _userRepository.UserWriteRepository.AddUser(new User
            {
                Username = entity.Username,
                Mail = entity.Mail,
                Password = password,
                IsActive = entity.IsActive
            });

            _mailSender.Send("example@gmail.com", "Added" + entity.Username, "Welcome ! " + entity.Username).Wait();

            ServiceResult result = new ServiceResult();

            if (!isUserAdded)
            {
                result.SetFailure(AddUserFailedMessage);
            }
            else
            {
                result.SetSuccess(AddUserSuccessMessage);
            }
            return result;
        }

        public ServiceResult RemoveUser(Guid id)
        {
            bool isUpdated = false;
            var user = _userRepository.UserReadRepository.GetUserById(id);
            if (user != null)
            {
                isUpdated = _userRepository.UserWriteRepository.DeleteUser(user);
            }

            ServiceResult result = new ServiceResult();

            if (!isUpdated)
            {
                result.SetFailure(DeleteUserFailedMessage);
            }
            else
            {
                result.SetSuccess(DeleteUserSuccessMessage);
            }
            return result;
        }

        private string CreateRandomPassword()
        {
            string charachters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPRSTUVWXYZ0123456789*!+-_.";
            Random random = new Random();
            string password = string.Empty;

            for (int i = 0; i < 16; i++)
            {
                int randomIndex = random.Next(0, charachters.Length);
                password += charachters[randomIndex];
            }

            return password;
        }
    }
}
