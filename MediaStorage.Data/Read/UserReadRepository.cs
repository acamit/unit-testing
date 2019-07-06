using MediaStorage.Common;
using MediaStorage.Common.ViewModels.User;
using MediaStorage.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStorage.Data.Read
{
    public class UserReadRepository
    {
        public List<UserViewModel> GetAllUsers()
        {
            using (var mediaContext = new MediaContext())
            {
                return mediaContext
                    .Users
                    .Select(s => new UserViewModel
                    {
                        Id = s.Id.ToString(),
                        Username = s.Username,
                        Mail = s.Mail,
                        IsActive = s.IsActive
                    }).ToList();
            }
        }

        public User GetUserById(Guid userId)
        {
            using (var mediaContext = new MediaContext())
            {
                return mediaContext.Users.Where(x => !x.IsDeleted).FirstOrDefault();
            }
        }

        public User GetByUserIdPassword(string userName, string password)
        {
            using (var mediaContext = new MediaContext())
            {
                return mediaContext.Users.Where(x => !x.IsDeleted && x.Username == userName && x.Password == password).FirstOrDefault();
            }
        }
        public User GetByUserName(string userName)
        {
            using (var mediaContext = new MediaContext())
            {
                return mediaContext
                    .Users
                    .Where(x => !x.IsDeleted && x.Username == userName).FirstOrDefault();
            }
        }
        public User GetByUserByEmail(string email)
        {
            using (var mediaContext = new MediaContext())
            {
                return mediaContext
                    .Users
                    .Where(x => !x.IsDeleted && x.Mail == email).FirstOrDefault();
            }
        }
    }
}
