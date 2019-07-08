using MediaStorage.Data.Read;
using MediaStorage.Data.Write;

namespace MediaStorage.Data.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        public DepartmentReadRepository DepartmentReadRepository { get; set; }
        public DepartmentWriteRepository DepartmentWriteRepository { get; set; }
    }
}
