using MediaStorage.Data.Read;
using MediaStorage.Data.Write;

namespace MediaStorage.Data.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        public DepartmentRepository(DepartmentReadRepository departmentReadRepository, DepartmentWriteRepository departmentWriteRepository)
        {
            DepartmentWriteRepository = departmentWriteRepository;
            DepartmentReadRepository = departmentReadRepository;
        }
        public IDepartmentReadRepository DepartmentReadRepository { get; set; }
        public IDepartmentWriteRepository DepartmentWriteRepository { get; set; }
    }
}
