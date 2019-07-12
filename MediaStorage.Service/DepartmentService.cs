namespace MediaStorage.Service
{
    using MediaStorage.Common;
    using MediaStorage.Common.Interfaces;
    using MediaStorage.Common.ViewModels.Department;
    using MediaStorage.Data.Repository;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static MediaStorage.Common.Constants;

    public class DepartmentService : IDepartmentService
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
            var departments = await _departmentRepository.DepartmentReadRepository.GetAllDepartments();
            if (departments == null || !departments.Any())
            {
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return departments;
        }

        public async Task<List<DepartmentListViewModel>> GetDepartmentsByLibraryId(int libraryId)
        {
            var departments = await _departmentRepository.DepartmentReadRepository.GetDepartmentsByLibraryId(libraryId);
            if (departments == null || !departments.Any())
            {
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return departments;
        }

        public async Task<DepartmentViewModel> GetDepartmentById(int id)
        {
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
            try
            {
                var department = await GetDepartmentById(libraryId);
                return (department != null && department.Id > 0);
            }
            catch (ResourceNotFoundException)
            {
                return false;
            }

        }

        public async Task<ServiceResult> AddDepartment(DepartmentViewModel entity)
        {
            var id = await _departmentRepository.DepartmentWriteRepository.AddDepartment(entity);
            ServiceResult result = new ServiceResult() { Id = id };
            if (id < 0)
            {
                result.SetFailure(DepartmentAddFailedMessage);
            }
            else
            {
                result.SetSuccess(DepartmentAddSuccessMessage);
            }
            return result;
        }

        public async Task<ServiceResult> UpdateDepartment(DepartmentViewModel entity)
        {
            var isUpdated = await _departmentRepository.DepartmentWriteRepository.UpdateDepartment(entity);
            ServiceResult result = new ServiceResult();
            if (!isUpdated)
            {
                result.SetFailure(UpdateDepartmentFailedMessage);
            }
            else
            {
                result.SetSuccess(UpdateDepartmentSuccessMessage);
            }
            return result;
        }

        public async Task<ServiceResult> RemoveDepartment(int id)
        {
            var isUpdated = await _departmentRepository.DepartmentWriteRepository.DeleteDepartment(id);
            ServiceResult result = new ServiceResult();
            if (!isUpdated)
            {
                result.SetFailure(DeleteDepartmentFailedMessage);
            }
            else
            {
                result.SetSuccess(DeleteDepartmentSuccessMessage);
            }
            return result;
        }
    }
}
