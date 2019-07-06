namespace MediaStorage.Service
{
    using MediaStorage.Common;
    using MediaStorage.Common.ViewModels.Department;
    using MediaStorage.Data.Read;
    using MediaStorage.Data.Repository;
    using MediaStorage.Data.Write;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static MediaStorage.Common.Constants;

    public class DepartmentService
    {
        private DepartmentRepository departmentRepository;
        private Logger logger;

        public DepartmentService()
        {
            departmentRepository = new DepartmentRepository();
            logger = new Logger();
        }

        public async Task<List<DepartmentListViewModel>> GetAllDepartments()
        {
            departmentRepository.DepartmentReadRepository = new DepartmentReadRepository();
            var departments = await departmentRepository.DepartmentReadRepository.GetAllDepartments();
            if (departments == null || !departments.Any())
            {
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return departments;
        }

        public async Task<List<DepartmentListViewModel>> GetDepartmentsByLibraryId(int libraryId)
        {
            departmentRepository.DepartmentReadRepository = new DepartmentReadRepository();
            var departments = await departmentRepository.DepartmentReadRepository.GetDepartmentsByLibraryId(libraryId);
            if (departments == null || !departments.Any())
            {
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return departments;
        }

        public async Task<DepartmentViewModel> GetDepartmentById(int id)
        {
            departmentRepository.DepartmentReadRepository = new DepartmentReadRepository();
            var department = await departmentRepository.DepartmentReadRepository.GetDepartmentById(id);

            if (department == null)
            {
                logger.Error(NoRecordsExists);
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
            departmentRepository.DepartmentWriteRepository = new DepartmentWriteRepository();
            var id = await departmentRepository.DepartmentWriteRepository.AddDepartment(entity);
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
            departmentRepository.DepartmentWriteRepository = new DepartmentWriteRepository();
            var isUpdated = await departmentRepository.DepartmentWriteRepository.UpdateDepartment(entity);
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
            departmentRepository.DepartmentWriteRepository = new DepartmentWriteRepository();
            var isUpdated = await departmentRepository.DepartmentWriteRepository.DeleteDepartment(id);
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
