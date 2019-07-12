using MediaStorage.Common;
namespace MediaStorage.Service
{
    using MediaStorage.Common.Interfaces;
    using MediaStorage.Common.ViewModels.Library;
    using MediaStorage.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static MediaStorage.Common.Constants;

    public class LibraryService : ILibraryService
    {

        private ILibraryRepository _libraryRepository;
        private IDepartmentRepository _departmentRepository;
        private ILogger _logger;

        public LibraryService(ILibraryRepository libraryRepository, IDepartmentRepository departmentRepository, ILogger logger)
        {
            _libraryRepository = libraryRepository;
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        public async Task<List<LibraryViewModel>> GetAllLibraries()
        {
            var libraries = await _libraryRepository.LibraryReadRepository.GetAllLibraries();
            if (libraries == null || !libraries.Any())
            {
                _logger.Error(NoRecordsExists);
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return libraries;
        }
        
        public async Task<List<CustomSelectListItem>> GetLibrariesAsSelectListItem(int? departmentId)
        {
            var libraries = await _libraryRepository.LibraryReadRepository.GetLibrariesAsSelectListItem(departmentId);
            if (libraries == null || !libraries.Any())
            {
                _logger.Error(NoRecordsExists);
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return libraries;
        }

        public async Task<LibraryViewModel> GetLibraryById(int id)
        {
            var library = await _libraryRepository.LibraryReadRepository.GetLibraryById(id);

            if (library == null)
            {
                _logger.Error(NoRecordsExists);
                throw new ResourceNotFoundException(NoRecordsExists);
            }
            return library;
        }

        public async Task<ServiceResult> AddLibrary(LibraryViewModel entity)
        {
            var id = await _libraryRepository.LibraryWriteRepository.AddLibrary(entity);
            ServiceResult result = new ServiceResult();
            result.Id = id;
            if (id < 0)
            {
                result.SetFailure(LibraryAddFailedMessage);
            }
            else
            {
                result.SetSuccess(LibraryAddSuccessMessage);
            }
            return result;
        }

        public async Task<ServiceResult> UpdateLibrary(LibraryViewModel entity)
        {
            var isUpdated = await _libraryRepository.LibraryWriteRepository.UpdateLibrary(entity);
            ServiceResult result = new ServiceResult();
            if (!isUpdated)
            {
                result.SetFailure(UpdateLibraryFailedMessage);
            }
            else
            {
                result.SetSuccess(UpdateLibrarySuccessMessage);
            }
            return result;
        }

        public async Task<ServiceResult> RemoveLibrary(int id)
        {
            
            var departments = await _departmentRepository.DepartmentReadRepository.GetDepartmentsByLibraryId(id);
            foreach (var department in departments)
            {
                var isDepartmentDeleted = await _departmentRepository.DepartmentWriteRepository.DeleteDepartment(department.Id);
                if (!isDepartmentDeleted)
                {
                    _logger.Error(DeleteDepartmentFailedMessage);
                    throw new Exception(DeleteDepartmentFailedMessage);
                }
            }
            return await DeleteLibrary(id);
        }

        private async Task<ServiceResult> DeleteLibrary(int id)
        {
            var isUpdated = await _libraryRepository.LibraryWriteRepository.DeleteLibrary(id);
            ServiceResult result = new ServiceResult();
            if (!isUpdated)
            {
                result.SetFailure(DeleteLibraryFailedMessage);
            }
            else
            {
                result.SetSuccess(DeleteLibrarySuccessMessage);
            }
            return result;
        }
    }
}
