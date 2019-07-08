namespace MediaStorage.Service
{
    using MediaStorage.Common;
    using MediaStorage.Common.Interfaces;
    using MediaStorage.Common.ViewModels.Department;
    using MediaStorage.Data.Read;
    using MediaStorage.Data.Repository;
    using MediaStorage.Data.Write;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static MediaStorage.Common.Constants;

    public class DepartmentService
    {
        private IDepartmentRepository _departmentRepository;
        private ILogger _logger;

        public DepartmentService(ILogger logger, IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        public async Task<List<DepartmentListViewModel>> GetAllDepartments()
        {
            _departmentRepository.DepartmentReadRepository = new DepartmentReadRepository();
            var departments = await _departmentRepository.DepartmentReadRepository.GetAllDepartments();
            if (departments == null || !departments.Any())
            {
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return departments;
        }

        public async Task<List<DepartmentListViewModel>> GetDepartmentsByLibraryId(int libraryId)
        {
            _departmentRepository.DepartmentReadRepository = new DepartmentReadRepository();
            var departments = await _departmentRepository.DepartmentReadRepository.GetDepartmentsByLibraryId(libraryId);
            if (departments == null || !departments.Any())
            {
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return departments;
        }

        public async Task<DepartmentViewModel> GetDepartmentById(int id)
        {
            _departmentRepository.DepartmentReadRepository = new DepartmentReadRepository();
            var department = await _departmentRepository.DepartmentReadRepository.GetDepartmentById(id);

            if (department == null)
            {
                _logger.Error(NoRecordsExists);
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return department;
        }

        public async Task<bool> HasDepartmentsByLibraryId(int libraryId)
        {
            var department = await GetDepartmentById(libraryId);
            return (department != null && department.Id > 0);
        }

        public async Task<ServiceResult> AddDepartment(DepartmentViewModel entity)
        {
            _departmentRepository.DepartmentWriteRepository = new DepartmentWriteRepository();
            var id = await _departmentRepository.DepartmentWriteRepository.AddDepartment(entity);
            ServiceResult result = new ServiceResult() { Id = id };
            if (id < 0)
            {
                result.SetFailure("Error while inserting department.");
            }
            else
            {
                result.SetSuccess("Department added successfully.");
            }
            return result;
        }

        public async Task<ServiceResult> UpdateDepartment(DepartmentViewModel entity)
        {
            _departmentRepository.DepartmentWriteRepository = new DepartmentWriteRepository();
            var isUpdated = await _departmentRepository.DepartmentWriteRepository.UpdateDepartment(entity);
            ServiceResult result = new ServiceResult();
            if (!isUpdated)
            {
                result.SetFailure("Error while updating department.");
            }
            else
            {
                result.SetSuccess("Department updated successfully.");
            }
            return result;
        }

        public async Task<ServiceResult> RemoveDepartment(int id)
        {
            _departmentRepository.DepartmentWriteRepository = new DepartmentWriteRepository();
            var isUpdated = await _departmentRepository.DepartmentWriteRepository.DeleteDepartment(id);
            ServiceResult result = new ServiceResult();
            if (!isUpdated)
            {
                result.SetFailure("Error while deleting department.");
            }
            else
            {
                result.SetSuccess("Department deleted successfully.");
            }
            return result;
        }
    }
}
