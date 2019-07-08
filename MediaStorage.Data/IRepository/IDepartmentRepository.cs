using MediaStorage.Data.Read;
using MediaStorage.Data.Write;

namespace MediaStorage.Data.Repository
{
    public interface IDepartmentRepository
    {
        DepartmentReadRepository DepartmentReadRepository { get; set; }
        DepartmentWriteRepository DepartmentWriteRepository { get; set; }
    }
}