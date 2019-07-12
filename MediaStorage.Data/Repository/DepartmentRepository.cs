using MediaStorage.Data.Read;
using MediaStorage.Data.Write;

namespace MediaStorage.Data.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        public DepartmentRepository(IDepartmentReadRepository departmentReadRepository, IDepartmentWriteRepository departmentWriteRepository)
        {
            DepartmentWriteRepository = departmentWriteRepository;
            DepartmentReadRepository = departmentReadRepository;
        }
        public IDepartmentReadRepository DepartmentReadRepository { get; set; }
        public IDepartmentWriteRepository DepartmentWriteRepository { get; set; }
    }
}
