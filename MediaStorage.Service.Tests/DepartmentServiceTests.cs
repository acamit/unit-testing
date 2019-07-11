using MediaStorage.Common;
using MediaStorage.Common.Interfaces;
using MediaStorage.Common.ViewModels.Department;
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

        #region GetAllDepartments
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
            var result = await _service.GetAllDepartments();
        }

        #endregion

        #region GetDepartmentsByLibraryId
        [TestMethod]
        public async Task GetDepartmentsByLibraryId_ShouldReturnAllDepartments()
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentsByLibraryId(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.AllDepartments);
            var result = await _service.GetDepartmentsByLibraryId(1);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetDepartmentsByLibraryId_ShouldThrowException()
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentsByLibraryId(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.AllDepartmentsEmptyList);
            var result = await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(async () =>
            {
                await _service.GetDepartmentsByLibraryId(1);
            });
            Assert.AreEqual(result.Message, Constants.NoRecordsExists);
        }
        [TestMethod]
        public async Task GetDepartmentsByLibraryId_ShouldCallRepositoryMethod()
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentsByLibraryId(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.AllDepartments);
            var result = await _service.GetDepartmentsByLibraryId(1);
            _departmentReadRepository.Verify(x => x.GetDepartmentsByLibraryId(It.IsAny<int>()), Times.Once, "Should Fetch data using repository");
        }
        #endregion

        #region GetDepartmentById
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task GetDepartmentById_ShouldReturnOneDepartment(int id)
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentById(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.Department);
            var result = await _service.GetDepartmentById(id);
            Assert.IsNotNull(result, "Should return a department object");
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task GetDepartmentById_ShouldThrowException(int id)
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentById(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.DepartmentNull);
            var result = await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(async () =>
            {
                await _service.GetDepartmentById(id);
            });
            Assert.AreEqual(result.Message, Constants.NoRecordsExists, "Should throw no record exists error");
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task GetDepartmentById_ShouldCallRepositoryMethod(int id)
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentById(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.Department);

            var result = await _service.GetDepartmentById(id);
            _departmentReadRepository.Verify(x => x.GetDepartmentById(It.IsAny<int>()), Times.Once, "Should Fetch data using repository");
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task GetDepartmentById_ShouldLogErrorMessage(int id)
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentById(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.DepartmentNull);
            //_loggerService.Setup(x => x.Error(It.IsAny<string>(), null, string.Empty,string.Empty, 0, string.Empty, null));
            var result = await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(async () =>
            {
                await _service.GetDepartmentById(id);
            });
            _loggerService.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once, "Should call logger service for error logging");
        }
        #endregion


        #region HasDepartmentsByLibraryId

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task HasDepartmentsByLibraryId_ShouldReturnTrue(int libraryId)
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentById(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.Department);
            _loggerService.Setup(x => x.Error(It.IsAny<string>(), null, string.Empty, string.Empty, 0, string.Empty, null));
            var result = await _service.HasDepartmentsByLibraryId(libraryId);
            Assert.IsTrue(result);
        }
        

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task HasDepartmentsByLibraryId_ShouldReturnFalse(int libraryId)
        {
            _departmentReadRepository.Setup(x => x.GetDepartmentById(It.IsAny<int>())).ReturnsAsync(DepartmentServiceTestData.DepartmentNull);
            _loggerService.Setup(x => x.Error(It.IsAny<string>(), null, string.Empty, string.Empty, 0, string.Empty, null));
            var result = await _service.HasDepartmentsByLibraryId(libraryId);
            Assert.IsFalse(result);
        }

        #endregion

        #region AddDepartment

        [TestMethod]
        public async Task AddDepartment_ShouldCallRepositoryMethod()
        {
            _departmentWriteRepository.Setup(x => x.AddDepartment(It.IsAny<DepartmentViewModel>())).ReturnsAsync(-1);
            var result = await _service.AddDepartment(DepartmentServiceTestData.Department);
            _departmentWriteRepository.Verify(x => x.AddDepartment(It.IsAny<DepartmentViewModel>()), Times.Once, "Should call add the department");
        }

        [TestMethod]
        public async Task AddDepartment_ShouldSetSuccessStatus()
        {
            _departmentWriteRepository.Setup(x => x.AddDepartment(It.IsAny<DepartmentViewModel>())).ReturnsAsync(1);
            var result = await _service.AddDepartment(DepartmentServiceTestData.Department);
            _departmentWriteRepository.Verify(x => x.AddDepartment(It.IsAny<DepartmentViewModel>()), Times.Once, "Should add the department");
            Assert.IsTrue(result.IsSuccessful, "Issuccessfull should be set to true");
            Assert.AreEqual(result.Message, Constants.DepartmentAddSuccessMessage);
        }
        [TestMethod]
        public async Task AddDepartment_ShouldSetFailStatus()
        {
            _departmentWriteRepository.Setup(x => x.AddDepartment(It.IsAny<DepartmentViewModel>())).ReturnsAsync(-1);
            var result = await _service.AddDepartment(DepartmentServiceTestData.Department);
            _departmentWriteRepository.Verify(x => x.AddDepartment(It.IsAny<DepartmentViewModel>()), Times.Once, "Should call add the department");
            Assert.IsFalse(result.IsSuccessful, "Issuccessfull should be set to false");
            Assert.AreEqual(result.Message, Constants.DepartmentAddFailedMessage, "Should set add department failed message correctly.");
        }


        #endregion

        #region UpdateDepartment

        [TestMethod]
        public async Task UpdateDepartment_ShouldUpdateDepartment()
        {
            _departmentWriteRepository.Setup(x => x.UpdateDepartment(It.IsAny<DepartmentViewModel>())).ReturnsAsync(true);
            var result = await _service.UpdateDepartment(DepartmentServiceTestData.Department);
            _departmentWriteRepository.Verify(x => x.UpdateDepartment(It.IsAny<DepartmentViewModel>()), Times.Once, "Should call update the department method");
            Assert.IsTrue(result.IsSuccessful , "Issuccessfull should be set to true");
            Assert.AreEqual(result.Message, Constants.UpdateDepartmentSuccessMessage, "Should set update department successs message correctly.");
        }

        [TestMethod]
        public async Task UpdateDepartment_ShouldSetSuccessMessage()
        {
            _departmentWriteRepository.Setup(x => x.UpdateDepartment(It.IsAny<DepartmentViewModel>())).ReturnsAsync(true);
            var result = await _service.UpdateDepartment(DepartmentServiceTestData.Department);
            Assert.IsTrue(result.IsSuccessful , "Issuccessfull should be set to True");
            Assert.AreEqual(result.Message, Constants.UpdateDepartmentSuccessMessage, "Should set update department success message correctly.");
        }


        [TestMethod]
        public async Task UpdateDepartment_ShouldSetFailedMessage()
        {
            _departmentWriteRepository.Setup(x => x.UpdateDepartment(It.IsAny<DepartmentViewModel>())).ReturnsAsync(false);
            var result = await _service.UpdateDepartment(DepartmentServiceTestData.Department);
            Assert.IsFalse(result.IsSuccessful, "Issuccessfull should be set to false");
            Assert.AreEqual(result.Message, Constants.UpdateDepartmentFailedMessage, "Should set update department failed message correctly.");
        }

        #endregion 

        #region RemoveDepartment
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task RemoveDepartment_ShouldRemoveOneDepartment(int id)
        {
            _departmentWriteRepository.Setup(x => x.DeleteDepartment(It.IsAny<int>())).ReturnsAsync(true);

            var result = await _service.RemoveDepartment(id);

            Assert.IsTrue(result.IsSuccessful, "Issuccessfull should be set to true");
            Assert.AreEqual(result.Message, Constants.DeleteDepartmentSuccessMessage, "Should set delete department successs message correctly.");
        }

       
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task RemoveDepartment_ShouldCallRepositoryMethod(int id)
        {
            var result = await _service.RemoveDepartment(id);
            _departmentWriteRepository.Verify(x => x.DeleteDepartment(It.IsAny<int>()), Times.Once, "Should delete data using repository");
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task RemoveDepartment_ShouldReturnFalse(int id)
        {
            _departmentWriteRepository.Setup(x => x.DeleteDepartment(It.IsAny<int>())).ReturnsAsync(false);

            var result = await _service.RemoveDepartment(id);

            Assert.IsFalse(result.IsSuccessful, "Issuccessfull should be set to false");
            Assert.AreEqual(result.Message, Constants.DeleteDepartmentFailedMessage, "Should set delete department failed message correctly.");
        }

        #endregion

        [TestInitialize]
        public void GetServiceInstance()
        {
            _service = new DepartmentService(_loggerService.Object, _departmentRepository.Object);
            _departmentRepository.Setup(x => x.DepartmentReadRepository).Returns(_departmentReadRepository.Object);
            _departmentRepository.Setup(x => x.DepartmentWriteRepository).Returns(_departmentWriteRepository.Object);

        }
    }
}
