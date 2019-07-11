using MediaStorage.Common;
using MediaStorage.Common.Interfaces;
using MediaStorage.Common.ViewModels.MaterialType;
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
    public class MaterialTypeServiceTests
    {
        private readonly Mock<ILogger> _loggerService;
        private readonly Mock<IMaterialReadRepository> _materialTypeReadRepository;
        private readonly Mock<IMaterialTypeWriteRepository> _materialTypeWriteRepository;
        private MaterialTypeService _service;
        public MaterialTypeServiceTests()
        {
            _loggerService = new Mock<ILogger>();
            _materialTypeReadRepository = new Mock<IMaterialReadRepository>();
            _materialTypeWriteRepository = new Mock<IMaterialTypeWriteRepository>();
        }

        #region GetAllMaterialTypes
        [TestMethod]
        public async Task GetAllMaterialTypes_ShouldReturnAllMaterials()
        {
            _materialTypeReadRepository.Setup(x => x.GetAllMaterials()).ReturnsAsync(MaterialTypeServiceTestData.AllMaterials);
            var result = await _service.GetAllMaterialTypes();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetAllMaterialTypes_ShouldCallRepositoryMethod()
        {
            _materialTypeReadRepository.Setup(x => x.GetAllMaterials()).ReturnsAsync(MaterialTypeServiceTestData.AllMaterials);
            var result = await _service.GetAllMaterialTypes();
            _materialTypeReadRepository.Verify(x => x.GetAllMaterials(), Times.Once, "Should Fetch data using repository");
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public async Task GetAllMaterialTypes_NoMaterialTypeFound_ShouldThrowException()
        {
            _materialTypeReadRepository.Setup(x => x.GetAllMaterials()).ReturnsAsync(MaterialTypeServiceTestData.AllMaterialsEmptyList);
            var result = await _service.GetAllMaterialTypes();
        }

        [TestMethod]
        public async Task GetAllMaterialTypes_NoMaterialTypeFound_ShouldLogMessage()
        {
            _materialTypeReadRepository.Setup(x => x.GetAllMaterials()).ReturnsAsync(MaterialTypeServiceTestData.AllMaterialsEmptyList);

            var result = await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(async () =>
            {
                await _service.GetAllMaterialTypes();
            });
            _loggerService.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once, "Should call logger service for error logging");
        }


        #endregion

        #region GetMaterialTypesAsSelectListItem

        [TestMethod]
        [DataRow(1)]
        [DataRow(null)]
        public async Task GetMaterialTypesAsSelectListItem_ReturnLibrariesSelectedItemList(int? id)
        {
            _materialTypeReadRepository.Setup(x => x.GetMaterialTypesAsSelectListItem(id)).ReturnsAsync(MaterialTypeServiceTestData.CustomSelectListItems);

            var result = await _service.GetMaterialTypesAsSelectListItem(id);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        [DataRow(1)]
        [DataRow(null)]
        public async Task GetMaterialTypesAsSelectListItem_EmptyList_ShouldThrowException(int? id)
        {
            _materialTypeReadRepository.Setup(x => x.GetMaterialTypesAsSelectListItem(id)).ReturnsAsync(MaterialTypeServiceTestData.CustomSelectListItemsEmpty);

            var result = await _service.GetMaterialTypesAsSelectListItem(id);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(null)]
        public async Task GetMaterialTypesAsSelectListItem_EmptyList_ShouldLogError(int? id)
        {
            _materialTypeReadRepository.Setup(x => x.GetMaterialTypesAsSelectListItem(id)).ReturnsAsync(LibraryServiceTestData.CustomSelectListItemsEmptyList);

            var result = await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(async () =>
            {
                await _service.GetMaterialTypesAsSelectListItem(id);
            });

            _loggerService.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once, "Should call logger service for error logging");
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(null)]
        public async Task GetMaterialTypesAsSelectListItem_NonEmptyListWithSelectedFalse_ShouldLogError(int? id)
        {
            _materialTypeReadRepository.Setup(x => x.GetMaterialTypesAsSelectListItem(id)).ReturnsAsync(MaterialTypeServiceTestData.CustomSelectListItemsWithSelectedFalse);

            var result = await Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                await _service.GetMaterialTypesAsSelectListItem(id);
            });

            _loggerService.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once, "Should call logger service for error logging");
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(null)]
        public async Task GetMaterialTypesAsSelectListItem_NonEmptyListWithSelectedFalse_ShouldRaiseSelectedSubCategoriesDoesNotExistErrorMessage(int? id)
        {
            _materialTypeReadRepository.Setup(x => x.GetMaterialTypesAsSelectListItem(id)).ReturnsAsync(MaterialTypeServiceTestData.CustomSelectListItemsWithSelectedFalse);

            var result = await Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                await _service.GetMaterialTypesAsSelectListItem(id);
            });

            Assert.AreEqual(result.Message, Constants.SelectedSubCategoriesDoesNotExistErrorMessage, "Should Raise subcategory not found exception");
        }

        #endregion

        #region GetMaterialTypeById

        [TestMethod]
        public async Task GetMaterialTypeById_ShouldReturnLibrary()
        {
            _materialTypeReadRepository.Setup(x => x.GetMaterialTypeById(It.IsAny<int>())).ReturnsAsync(MaterialTypeServiceTestData.MaterialType);

            var result = await _service.GetMaterialTypeById(1);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException), "Should throw exception if no data found")]
        public async Task GetMaterialTypeById_ShouldThrowException()
        {
            _materialTypeReadRepository.Setup(x => x.GetMaterialTypeById(It.IsAny<int>())).ReturnsAsync((MaterialTypeViewModel)null);

            var result = await _service.GetMaterialTypeById(1);
        }

        [TestMethod]
        public async Task GetMaterialTypeById_ShouldLogError()
        {
            _materialTypeReadRepository.Setup(x => x.GetMaterialTypeById(It.IsAny<int>())).ReturnsAsync((MaterialTypeViewModel)null);

            await Assert.ThrowsExceptionAsync<ResourceNotFoundException>(async () =>
            {
                await _service.GetMaterialTypeById(1);
            });

            _loggerService.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once, "Should call logger service for error logging");
        }
        #endregion

        #region AddMaterialType
        [TestMethod]
        public async Task AddMaterialType_ShouldCallRepositoryMethod()
        {
            _materialTypeWriteRepository.Setup(x => x.AddMaterialType(It.IsAny<MaterialTypeViewModel>())).ReturnsAsync(21);

            var result = await _service.AddMaterialType(MaterialTypeServiceTestData.MaterialType);

            _materialTypeWriteRepository.Verify(x => x.AddMaterialType(It.IsAny<MaterialTypeViewModel>()), Times.Once, "Should call add the material type on repository");
        }

        [TestMethod]
        public async Task AddMaterialType_ShouldSetSuccessStatus()
        {
            _materialTypeWriteRepository.Setup(x => x.AddMaterialType(It.IsAny<MaterialTypeViewModel>())).ReturnsAsync(1);

            var result = await _service.AddMaterialType(MaterialTypeServiceTestData.MaterialType);

            Assert.IsTrue(result.IsSuccessful, "Issuccessfull should be set to true");
            Assert.AreEqual(result.Message, Constants.MaterialAddSuccessMessage);
        }
        [TestMethod]
        public async Task AddMaterialType_ShouldSetFailStatus()
        {
            _materialTypeWriteRepository.Setup(x => x.AddMaterialType(It.IsAny<MaterialTypeViewModel>())).ReturnsAsync(-1);

            var result = await _service.AddMaterialType(MaterialTypeServiceTestData.MaterialType);
            
            Assert.IsFalse(result.IsSuccessful, "Issuccessfull should be set to false");
            Assert.AreEqual(result.Message, Constants.MaterialAddFailedMessage, "Should set add department failed message correctly.");
        }

        #endregion

        #region UpdateMaterialType
        [TestMethod]
        public async Task UpdateMaterialType_ShouldUpdateDepartment()
        {
            _materialTypeWriteRepository.Setup(x => x.UpdateMaterialType(It.IsAny<MaterialTypeViewModel>())).ReturnsAsync(true);

            var result = await _service.UpdateMaterialType(MaterialTypeServiceTestData.MaterialType);

            _materialTypeWriteRepository.Verify(x => x.UpdateMaterialType(It.IsAny<MaterialTypeViewModel>()), Times.Once, "Should call update the library method");
        }

        [TestMethod]
        public async Task UpdateMaterialType_ShouldSetSuccessMessage()
        {
            _materialTypeWriteRepository.Setup(x => x.UpdateMaterialType(It.IsAny<MaterialTypeViewModel>())).ReturnsAsync(true);

            var result = await _service.UpdateMaterialType(MaterialTypeServiceTestData.MaterialType);

            Assert.IsTrue(result.IsSuccessful, "Issuccessfull should be set to true");
            Assert.AreEqual(result.Message, Constants.UpdateMaterialSuccessMessage, "Should set update library success message correctly.");
        }


        [TestMethod]
        public async Task UpdateMaterialType_ShouldSetFailedMessage()
        {
            _materialTypeWriteRepository.Setup(x => x.UpdateMaterialType(It.IsAny<MaterialTypeViewModel>())).ReturnsAsync(false);

            var result = await _service.UpdateMaterialType(MaterialTypeServiceTestData.MaterialType);

            Assert.IsFalse(result.IsSuccessful, "Issuccessfull should be set to false");
            Assert.AreEqual(result.Message, Constants.UpdateMaterialFailedMessage, "Should set update library failed message correctly.");
        }

        #endregion

        #region RemoveMaterialType
        [TestMethod]
        [DataRow(1)]
        [DataRow(3)]
        public async Task RemoveMaterialType_ShouldCallMaterialRepository(int id)
        {
            _materialTypeWriteRepository.Setup(x => x.RemoveMaterialType(It.IsAny<int>())).ReturnsAsync(true);
            var result = await _service.RemoveMaterialType(id);
            Assert.IsTrue(result.IsSuccessful, "Issuccessfull should be set to true");
            Assert.AreEqual(result.Message, Constants.DeleteMaterialSuccessMessage, "Should set delete materialtype successs message correctly.");
        }


        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public async Task RemoveMaterialType_ShouldCallRepositoryMethod(int id)
        {
            var result = await _service.RemoveMaterialType(id);
            _materialTypeWriteRepository.Verify(x => x.RemoveMaterialType(It.IsAny<int>()), Times.Once, "Should delete data using repository");
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public async Task RemoveMaterialType_ShouldReturnFalse(int id)
        {
            _materialTypeWriteRepository.Setup(x => x.RemoveMaterialType(It.IsAny<int>())).ReturnsAsync(false);
            var result = await _service.RemoveMaterialType(id);
            Assert.IsFalse(result.IsSuccessful, "Issuccessfull should be set to false");
            Assert.AreEqual(result.Message, Constants.DeleteMaterialFailedMessage, "Should set delete materialtype failed message correctly.");
        }

        #endregion
        [TestInitialize]
        public void GetServiceInstance()
        {
            _service = new MaterialTypeService(_materialTypeReadRepository.Object, _materialTypeWriteRepository.Object, _loggerService.Object);
        }
    }
}
