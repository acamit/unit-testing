using MediaStorage.Common;
namespace MediaStorage.Service
{
    using MediaStorage.Common.ViewModels.Library;
    using MediaStorage.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static MediaStorage.Common.Constants;

    public class LibraryService
    {

        private LibraryRepository libraryRepository;
        private DepartmentRepository departmentRepository;
        private Logger logger;

        public LibraryService()
        {
            logger = new Logger();
        }

        public async Task<List<LibraryViewModel>> GetAllLibraries()
        {
            libraryRepository = new LibraryRepository();
            var libraries = await libraryRepository.LibraryReadRepository.GetAllLibraries();
            if (libraries == null || !libraries.Any())
            {
                logger.Error(NoRecordsExists);
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return libraries;
        }
        
        public async Task<List<CustomSelectListItem>> GetLibrariesAsSelectListItem(int? departmentId)
        {
            libraryRepository = new LibraryRepository();
            var libraries = await libraryRepository.LibraryReadRepository.GetLibrariesAsSelectListItem(departmentId);
            if (libraries == null || !libraries.Any())
            {
                logger.Error(NoRecordsExists);
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return libraries;
        }

        public async Task<LibraryViewModel> GetLibraryById(int id)
        {
            libraryRepository = new LibraryRepository();
            var library = await libraryRepository.LibraryReadRepository.GetLibraryById(id);

            if (library == null)
            {
                logger.Error(NoRecordsExists);
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return library;
        }

        public async Task<ServiceResult> AddLibrary(LibraryViewModel entity)
        {
            libraryRepository = new LibraryRepository();
            var id = await libraryRepository.LibraryWriteRepository.AddLibrary(entity);
            ServiceResult result = new ServiceResult();
            result.Id = id;
            if (id < 0)
            {
                result.SetFailure("Error while inserting library.");
            }
            else
            {
                result.SetSuccess("Library added successfully.");
            }
            return result;
        }

        public async Task<ServiceResult> UpdateLibrary(LibraryViewModel entity)
        {
            libraryRepository = new LibraryRepository();
            var isUpdated = await libraryRepository.LibraryWriteRepository.UpdateLibrary(entity);
            ServiceResult result = new ServiceResult();
            if (!isUpdated)
            {
                result.SetFailure("Error while updating library.");
            }
            else
            {
                result.SetSuccess("Library updated successfully.");
            }
            return result;
        }

        public async Task<ServiceResult> RemoveLibrary(int id)
        {
            departmentRepository = new DepartmentRepository();
            departmentRepository.DepartmentReadRepository = new Data.Read.DepartmentReadRepository();
            departmentRepository.DepartmentWriteRepository = new Data.Write.DepartmentWriteRepository();


            var departments = await departmentRepository.DepartmentReadRepository.GetDepartmentsByLibraryId(id);
            foreach (var department in departments)
            {
                var isDepartmentDeleted = await departmentRepository.DepartmentWriteRepository.DeleteDepartment(department.Id);
                if (!isDepartmentDeleted)
                {
                    logger.Error("Error while deleting department");
                    throw new Exception("Error while deleting department");
                }
            }
            return await DeleteLibrary(id);
        }

        private async Task<ServiceResult> DeleteLibrary(int id)
        {
            libraryRepository = new LibraryRepository();
            var isUpdated = await libraryRepository.LibraryWriteRepository.DeleteLibrary(id);
            ServiceResult result = new ServiceResult();
            if (!isUpdated)
            {
                result.SetFailure("Error while deleting library.");
            }
            else
            {
                result.SetSuccess("Library deleted successfully.");
            }
            return result;
        }
    }
}
