using MediaStorage.Common;
using MediaStorage.Common.Interfaces;
using MediaStorage.Common.ViewModels.Library;
using MediaStorage.Data.Read;
using MediaStorage.Data.Repository;
using MediaStorage.Data.Write;
using MediaStorage.Service.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaStorage.Service.Tests
{
    [TestClass]
    public class LibraryServiceTests
    {
        private readonly Mock<ILibraryReadRepository> _libraryReadRepository;
        private readonly Mock<ILibraryWriteRepository> _libraryWriteRepository;
        private readonly Mock<ILibraryRepository> _libraryRepository;
        private readonly Mock<IDepartmentRepository> _departmentRepository;
        private readonly Mock<IDepartmentReadRepository> _departmentReadRepository;
        private readonly Mock<IDepartmentWriteRepository> _departmentWriteRepository;
        private readonly Mock<ILogger> _loggerService;
        private LibraryService _service;

        public LibraryServiceTests()
        {
            _libraryRepository = new Mock<ILibraryRepository>();
            _libraryWriteRepository = new Mock<ILibraryWriteRepository>();
            _libraryReadRepository = new Mock<ILibraryReadRepository>();
            _departmentRepository = new Mock<IDepartmentRepository>();
            _departmentReadRepository = new Mock<IDepartmentReadRepository>();
            _departmentWriteRepository = new Mock<IDepartmentWriteRepository>();
            _loggerService = new Mock<ILogger>();
        }

        #region GetAllLibraries
        [TestMethod]
        public async Task GetAllLibraries_ShouldReturnAllDepartments()
        {
            _libraryReadRepository.Setup(x => x.GetAllLibraries()).ReturnsAsync(LibraryServiceTestData.AllLibraries);
            var result = await _service.GetAllLibraries();
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetAllLibraries_ShouldCallRepositoryMethod()
        {
            _libraryReadRepository.Setup(x => x.GetAllLibraries()).ReturnsAsync(LibraryServiceTestData.AllLibraries);
            var result = await _service.GetAllLibraries();
            _libraryReadRepository.Verify(x => x.GetAllLibraries(), Times.Once, "Should Fetch data using repository");
        }
        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public async Task GetAllLibraries_ShouldThrowException()
        {
            _libraryReadRepository.Setup(x => x.GetAllLibraries()).ReturnsAsync(LibraryServiceTestData.AllLibrariesEmptyList);
            var result = await _service.GetAllLibraries();
        }
        [TestMethod]
        public async Task GetDepartmentById_ShouldLogErrorMessage()
        {
            _libraryReadRepository.Setup(x => x.GetAllLibraries()).ReturnsAsync(LibraryServiceTestData.AllLibrariesEmptyList);
            var result = await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(async () =>
            {
                await _service.GetAllLibraries();
            });
            _loggerService.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once, "Should call logger service for error logging");
        }
        #endregion

        #region GetLibrariesAsSelectListItem

        [TestMethod]
        [DataRow(1)]
        [DataRow(null)]
        public async Task GetLibrariesAsSelectListItem_ReturnLibrariesSelectedItemList(int? id)
        {
            _libraryReadRepository.Setup(x => x.GetLibrariesAsSelectListItem(id)).ReturnsAsync(LibraryServiceTestData.CustomSelectListItems);
            var result = await _service.GetLibrariesAsSelectListItem(id);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        [DataRow(1)]
        [DataRow(null)]
        public async Task GetLibrariesAsSelectListItem_ShouldThrowException(int? id)
        {
            _libraryReadRepository.Setup(x => x.GetLibrariesAsSelectListItem(id)).ReturnsAsync(LibraryServiceTestData.CustomSelectListItemsEmptyList);
            var result = await _service.GetLibrariesAsSelectListItem(id);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(null)]
        public async Task GetLibrariesAsSelectListItem_ShouldLogError(int? id)
        {
            _libraryReadRepository.Setup(x => x.GetLibrariesAsSelectListItem(id)).ReturnsAsync(LibraryServiceTestData.CustomSelectListItemsEmptyList);

            var result = await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(async () =>
            {
                await _service.GetLibrariesAsSelectListItem(id);
            });
            _loggerService.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once, "Should call logger service for error logging");
        }
        #endregion

        #region GetLibraryById

        [TestMethod]
        public async Task GetLibraryById_ShouldReturnLibrary()
        {
            _libraryReadRepository.Setup(x => x.GetLibraryById(It.IsAny<int>())).ReturnsAsync(LibraryServiceTestData.Library);
            var result = await _service.GetLibraryById(1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException), "Should throw exception if no data found")]
        public async Task GetLibraryById_ShouldThrowException()
        {
            _libraryReadRepository.Setup(x => x.GetLibraryById(It.IsAny<int>())).ReturnsAsync((LibraryViewModel)null);
            var result = await _service.GetLibraryById(1);
        }

        [TestMethod]
        public async Task GetLibraryById_ShouldLogError()
        {
            _libraryReadRepository.Setup(x => x.GetLibraryById(It.IsAny<int>())).ReturnsAsync((LibraryViewModel)null);
            await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(async () =>
            {
                await _service.GetLibraryById(1);
            });
            _loggerService.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once, "Should call logger service for error logging");
        }
        #endregion

        #region AddLibrary
        [TestMethod]
        public async Task AddLibrary_ShouldCallRepositoryMethod()
        {
            _libraryWriteRepository.Setup(x => x.AddLibrary(It.IsAny<LibraryViewModel>())).ReturnsAsync(-1);
            var result = await _service.AddLibrary(LibraryServiceTestData.Library);
            _libraryWriteRepository.Verify(x => x.AddLibrary(It.IsAny<LibraryViewModel>()), Times.Once, "Should call add the department");
        }

        [TestMethod]
        public async Task AddLibrary_ShouldSetSuccessStatus()
        {
            _libraryWriteRepository.Setup(x => x.AddLibrary(It.IsAny<LibraryViewModel>())).ReturnsAsync(1);
            var result = await _service.AddLibrary(LibraryServiceTestData.Library);
            Assert.IsTrue(result.IsSuccessful, "Issuccessfull should be set to true");
            Assert.AreEqual(result.Message, Constants.LibraryAddSuccessMessage);
        }
        [TestMethod]
        public async Task AddLibrary_ShouldSetFailStatus()
        {
            _libraryWriteRepository.Setup(x => x.AddLibrary(It.IsAny<LibraryViewModel>())).ReturnsAsync(-1);
            var result = await _service.AddLibrary(LibraryServiceTestData.Library);
            Assert.IsFalse(result.IsSuccessful, "Issuccessfull should be set to false");
            Assert.AreEqual(result.Message, Constants.LibraryAddFailedMessage, "Should set add department failed message correctly.");
        }

        #endregion

        #region UpdateLibrary
        [TestMethod]
        public async Task UpdateLibrary_ShouldUpdateDepartment()
        {
            _libraryWriteRepository.Setup(x => x.UpdateLibrary(It.IsAny<LibraryViewModel>())).ReturnsAsync(true);
            var result = await _service.UpdateLibrary(LibraryServiceTestData.Library);
            _libraryWriteRepository.Verify(x => x.UpdateLibrary(It.IsAny<LibraryViewModel>()), Times.Once, "Should call update the library method");
        }

        [TestMethod]
        public async Task UpdateLibrary_ShouldSetSuccessMessage()
        {
            _libraryWriteRepository.Setup(x => x.UpdateLibrary(It.IsAny<LibraryViewModel>())).ReturnsAsync(true);
            var result = await _service.UpdateLibrary(LibraryServiceTestData.Library);
            Assert.IsTrue(result.IsSuccessful, "Issuccessfull should be set to true");
            Assert.AreEqual(result.Message, Constants.UpdateLibrarySuccessMessage, "Should set update library success message correctly.");
        }


        [TestMethod]
        public async Task UpdateLibrary_ShouldSetFailedMessage()
        {
            _libraryWriteRepository.Setup(x => x.UpdateLibrary(It.IsAny<LibraryViewModel>())).ReturnsAsync(false);
            var result = await _service.UpdateLibrary(LibraryServiceTestData.Library);
            Assert.IsFalse(result.IsSuccessful, "Issuccessfull should be set to false");
            Assert.AreEqual(result.Message, Constants.UpdateLibraryFailedMessage, "Should set update library failed message correctly.");
        }

        #endregion

        #region RemoveLibrary
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task RemoveLibrary_MultipleDepartmentsForLibrary_ShouldRemoveOneLibrary(int id)
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentsByLibraryId(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.AllDepartments);
            _departmentWriteRepository.Setup(x => x.DeleteDepartment(It.IsAny<int>())).ReturnsAsync(true);
            _libraryWriteRepository.Setup(x => x.DeleteLibrary(It.IsAny<int>())).ReturnsAsync(true);

            var result = await _service.RemoveLibrary(id);

            Assert.IsTrue(result.IsSuccessful, "Issuccessfull should be set to true");
            Assert.AreEqual(result.Message, Constants.DeleteLibrarySuccessMessage, "Should set delete library successs message correctly.");
        }

        [TestMethod]
        public async Task RemoveLibrary_MultipleDepartmentsForLibrary_ShouldDeleteAllDepartments()
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentsByLibraryId(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.AllDepartments);
            _departmentWriteRepository.Setup(x => x.DeleteDepartment(It.IsAny<int>())).ReturnsAsync(true);
            _libraryWriteRepository.Setup(x => x.DeleteLibrary(It.IsAny<int>())).ReturnsAsync(true);

            var result = await _service.RemoveLibrary(2);
            _departmentWriteRepository.Verify(x => x.DeleteDepartment(It.IsAny<int>()), Times.Exactly(2), "Should delete all departments before deleting Repository");
        }


        [TestMethod]
        public async Task RemoveLibrary_DeleteDepartmentFails_ShouldThrowException()
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentsByLibraryId(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.AllDepartments);
            _departmentWriteRepository.Setup(x => x.DeleteDepartment(It.IsAny<int>())).ReturnsAsync(false);
            _libraryWriteRepository.Setup(x => x.DeleteLibrary(It.IsAny<int>())).ReturnsAsync(true);
            var result = await Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                await _service.RemoveLibrary(1);
            });
            Assert.AreEqual(result.Message, Constants.DeleteDepartmentFailedMessage);
        }
        [TestMethod]
        public async Task RemoveLibrary_DeleteDepartmentFails_ShouldLogError()
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentsByLibraryId(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.AllDepartments);
            _departmentWriteRepository.Setup(x => x.DeleteDepartment(It.IsAny<int>())).ReturnsAsync(false);
            _libraryWriteRepository.Setup(x => x.DeleteLibrary(It.IsAny<int>())).ReturnsAsync(true);
            var result = await Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                await _service.RemoveLibrary(1);
            });
            _loggerService.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once, "Should call logger service for error logging");
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task RemoveLibrary_DepartSuccessfullDeleted_ShouldCallRepositoryMethod(int id)
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentsByLibraryId(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.AllDepartments);
            _departmentWriteRepository.Setup(x => x.DeleteDepartment(It.IsAny<int>())).ReturnsAsync(true);
            _libraryWriteRepository.Setup(x => x.DeleteLibrary(It.IsAny<int>())).ReturnsAsync(true);

            var result = await _service.RemoveLibrary(id);

            _libraryWriteRepository.Verify(x => x.DeleteLibrary(It.IsAny<int>()), Times.Once, "Should delete data using repository");
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task RemoveLibrary_RemoveLibraryFails_ShouldReturnFalse(int id)
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentsByLibraryId(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.AllDepartments);
            _departmentWriteRepository.Setup(x => x.DeleteDepartment(It.IsAny<int>())).ReturnsAsync(true);
            _libraryWriteRepository.Setup(x => x.DeleteLibrary(It.IsAny<int>())).ReturnsAsync(false);

            var result = await _service.RemoveLibrary(id);

            Assert.IsFalse(result.IsSuccessful, "Issuccessfull should be set to false");
            Assert.AreEqual(result.Message, Constants.DeleteLibraryFailedMessage, "Should set delete library successs message correctly.");
        }

        #endregion


        [TestInitialize]
        public void GetServiceInstance()
        {
            _service = new LibraryService(_libraryRepository.Object, _departmentRepository.Object, _loggerService.Object);
            _libraryRepository.Setup(x => x.LibraryReadRepository).Returns(_libraryReadRepository.Object);
            _libraryRepository.Setup(x => x.LibraryWriteRepository).Returns(_libraryWriteRepository.Object);
            _departmentRepository.Setup(x => x.DepartmentReadRepository).Returns(_departmentReadRepository.Object);
            _departmentRepository.Setup(x => x.DepartmentWriteRepository).Returns(_departmentWriteRepository.Object);
        }
    }
}
