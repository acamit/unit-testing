using MediaStorage.Data.Read;

namespace MediaStorage.Data.Repository
{    public class UserRepository
    {
        public UserReadRepository userReadRepository = new UserReadRepository();
        public UserWriteRepository userWriteRepository = new UserWriteRepository();
    }
}
