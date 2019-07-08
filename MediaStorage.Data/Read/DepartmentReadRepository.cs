namespace MediaStorage.Data.Read
{
    using MediaStorage.Common.ViewModels.Department;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class DepartmentReadRepository : IDepartmentReadRepository
    {
        public async Task<List<DepartmentListViewModel>> GetAllDepartments()
        {
            using (var mediaContext = new MediaContext())
            {
                return await mediaContext.Departments
                .Select(s => new DepartmentListViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    LibraryName = s.Library.Name
                }).ToListAsync();
            }
        }

        public async Task<List<DepartmentListViewModel>> GetDepartmentsByLibraryId(int libraryId)
        {
            using (var mediaContext = new MediaContext())
            {
                return await mediaContext.Departments
                    .Where(x => x.LibraryId == libraryId)
                .Select(s => new DepartmentListViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    LibraryName = s.Library.Name
                }).ToListAsync();
            }
        }

        public async Task<DepartmentViewModel> GetDepartmentById(int departmentId)
        {
            using (var mediaContext = new MediaContext())
            {
                return await mediaContext.Departments
                    .Where(x => x.Id == departmentId)
                .Select(s => new DepartmentViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    LibraryId = s.Library.Id
                }).FirstOrDefaultAsync();
            }
        }
        
    }
}
