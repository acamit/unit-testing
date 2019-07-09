using MediaStorage.Common;
using MediaStorage.Common.Interfaces;
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
        private Mock<ILibraryReadRepository> _libraryReadRepository;
        private Mock<ILibraryWriteRepository> _libraryWriteRepository;
        private Mock<ILibraryRepository> _libraryRepository;
        private Mock<IDepartmentRepository> _departmentRepository;
        private readonly Mock<IDepartmentReadRepository> _departmentReadRepository;
        private readonly Mock<IDepartmentWriteRepository> _departmentWriteRepository;
        private Mock<ILogger> _loggerService;
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
            _loggerService.Setup(x => x.Error(It.IsAny<string>(), null, string.Empty, string.Empty, 0, string.Empty, null));
            var result = await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(async () =>
            {
                await _service.GetAllLibraries();
            });
            _loggerService.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once, "Should call logger service for error logging");
        }
        #endregion

        #region GetLibrariesAsSelectListItem

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
