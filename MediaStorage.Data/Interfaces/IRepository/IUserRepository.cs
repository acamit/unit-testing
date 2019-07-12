using MediaStorage.Data.Read;

namespace MediaStorage.Data.Interfaces.IRepository
{
    public interface IUserRepository
    {
        IUserReadRepository UserReadRepository { get; set; }
        IUserWriteRepository UserWriteRepository { get; set; }
    }
}
