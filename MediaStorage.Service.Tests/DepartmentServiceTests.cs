using MediaStorage.Common;
using MediaStorage.Common.Interfaces;
using MediaStorage.Data.Read;
using MediaStorage.Data.Repository;
using MediaStorage.Data.Write;
using MediaStorage.Service.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace MediaStorage.Service.Tests
{
    [TestClass]
    public class DepartmentServiceTests
    {
        private readonly Mock<ILogger> _loggerService;
        private readonly Mock<IDepartmentRepository> _departmentRepository;
        private readonly Mock<IDepartmentReadRepository> _departmentReadRepository;
        private readonly Mock<IDepartmentWriteRepository> _departmentWriteRepository;
        private DepartmentService _service;
        public DepartmentServiceTests()
        {
            _loggerService = new Mock<ILogger>();
            _departmentRepository = new Mock<IDepartmentRepository>();
            _departmentReadRepository = new Mock<IDepartmentReadRepository>();
            _departmentWriteRepository = new Mock<IDepartmentWriteRepository>();
        }
        [TestMethod]
        public async Task GetAllDepartments_ShouldReturnAllDepartments()
        {
            _departmentReadRepository.Setup(x => x.GetAllDepartments()).ReturnsAsync(DepartmentServiceTestData.AllDepartments);
            var result = await _service.GetAllDepartments();
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetAllDepartments_ShouldCallRepositoryMethod()
        {
            _departmentReadRepository.Setup(x => x.GetAllDepartments()).ReturnsAsync(DepartmentServiceTestData.AllDepartments);
            var result = await _service.GetAllDepartments();
            _departmentReadRepository.Verify(x => x.GetAllDepartments(), Times.Once, "Should Fetch data using repository");
        }
        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public async Task GetAllDepartments_ShouldThrowException()
        {
            _departmentReadRepository.Setup(x => x.GetAllDepartments()).ReturnsAsync(DepartmentServiceTestData.AllDepartmentsEmptyList);
            var result =   await _service.GetAllDepartments();
        }
        [TestMethod]
        public async Task GetDepartmentsByLibraryId_ShouldReturnAllDepartments()
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentsByLibraryId(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.AllDepartments);
            var result = await _service.GetDepartmentsByLibraryId(1);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public async Task GetDepartmentsByLibraryId_ShouldThrowException()
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentsByLibraryId(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.AllDepartmentsEmptyList);
            var result = await _service.GetDepartmentsByLibraryId(1);
        }
        [TestMethod]
        public async Task GetDepartmentsByLibraryId_ShouldCallRepositoryMethod()
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentsByLibraryId(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.AllDepartments);
            var result = await _service.GetDepartmentsByLibraryId(1);
            _departmentReadRepository.Verify(x => x.GetDepartmentsByLibraryId(It.IsAny<int>()), Times.Once, "Should Fetch data using repository");
        }

        [TestInitialize]
        public void GetServiceInstance()
        {
            _service = new DepartmentService(_loggerService.Object, _departmentRepository.Object);
            _departmentRepository.Setup(x => x.DepartmentReadRepository).Returns(_departmentReadRepository.Object);

        }
    }
}
