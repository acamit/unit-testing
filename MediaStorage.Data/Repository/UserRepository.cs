using MediaStorage.Data.Interfaces.IRepository;
using MediaStorage.Data.Read;

namespace MediaStorage.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository)
        {
            UserReadRepository = userReadRepository;
            UserWriteRepository = userWriteRepository;
        }

        public IUserReadRepository UserReadRepository { get; set; }
        public IUserWriteRepository UserWriteRepository { get; set; }

    }

}
