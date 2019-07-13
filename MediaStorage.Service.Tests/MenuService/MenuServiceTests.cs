using MediaStorage.Config;
using MediaStorage.Data;
using MediaStorage.Data.Entities;
using MediaStorage.Service.Tests.MenuServiceTests;
using MediaStorage.Service.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MediaStorage.Service.Tests.MenuService
{
    [TestClass]
    public class MenuServiceTests
    {
        private readonly Mock<IConfigurationProvider> _configurationProvider;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IRepository<Menu>> _menuRepository;
        private readonly Mock<IRepository<MenuItem>> _menuItemRepository;
        private readonly Mock<IMenuServiceMockHelper> _menuServiceMockHelper;
        private MenuServiceWrapper _menuService;
        public MenuServiceTests()
        {
            _uow = new Mock<IUnitOfWork>();
            _configurationProvider = new Mock<IConfigurationProvider>();
            _menuRepository = new Mock<IRepository<Menu>>();
            _menuItemRepository = new Mock<IRepository<MenuItem>>();
            _menuServiceMockHelper = new Mock<IMenuServiceMockHelper>();
        }

        #region GetAllMenus

        [TestMethod]
        public void GetAllMenus_ConfigReturnsFalse_ShouldReturnBasedOnConfigurationSettings()
        {
            var result = _menuService.GetAllMenus();
            _configurationProvider.Verify(x => x.CanGetAllMenus(), Times.Once, "Should use configuration provider for fetching the config settings");
        }

        [TestMethod]
        public void GetAllMenus_ConfigReturnsTrue_ShouldReturnListOfMenus()
        {
            _configurationProvider.Setup(x => x.CanGetAllMenus()).Returns("true");
            _menuRepository.Setup(x => x.GetAll()).Returns(MenuServiceTestData.Menus.AsQueryable());
            var result = _menuService.GetAllMenus();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAllMenus_ConfigReturnsFalse_ShouldReturnNull()
        {
            _configurationProvider.Setup(x => x.CanGetAllMenus()).Returns("false");
            var result = _menuService.GetAllMenus();
            Assert.IsNull(result);
        }

        #endregion

        #region GetMenuById
        [TestMethod]
        public void GetMenuById_MenuExists_ShouldNotReturnNull()
        {
            _menuRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(MenuServiceTestData.Menu);
            var result = _menuService.GetMenuById(1);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void GetMenuById_MenuDoesNotExists_ShouldReturnNull()
        {
            _menuRepository.Setup(x => x.Find(It.IsAny<int>())).Returns((Menu)null);
            var result = _menuService.GetMenuById(1);
            Assert.IsNull(result);
        }
        #endregion

        #region AddMenu
        [TestMethod]
        public void AddMenu_ShouldAddNewMenuUsingRepository()
        {
            _menuServiceMockHelper.Setup(x => x.GetAddResult(It.IsAny<bool>())).Returns(MenuServiceTestData.SuccessServiceResult);
            var result = _menuService.AddMenu(MenuServiceTestData.NewMenu);
            _menuRepository.Verify(x => x.Add(It.IsAny<Menu>()), Times.Once, "Should Add new item using repository");
        }

        [TestMethod]
        public void AddMenu_ShouldCommitChanges()
        {
            _menuServiceMockHelper.Setup(x => x.GetAddResult(It.IsAny<bool>())).Returns(MenuServiceTestData.SuccessServiceResult);
            var result = _menuService.AddMenu(MenuServiceTestData.NewMenu);
            _uow.Verify(x => x.Commit(), Times.Once, "Should commit changes after adding the menu.");
        }

        [TestMethod]
        public void AddMenu_ItemAdded_ShouldGetServiceResult()
        {
            var result = _menuService.AddMenu(MenuServiceTestData.NewMenu);

            _menuServiceMockHelper.Verify(x => x.GetAddResult(It.IsAny<bool>()), Times.Once, "Should get result from service result");
        }

        #endregion

        #region UpdateMenu
        [TestMethod]
        public void UpdateMenu_ShouldUpdateMenuUsingRepository()
        {
            _menuServiceMockHelper.Setup(x => x.GetUpdateResult(It.IsAny<bool>())).Returns(MenuServiceTestData.SuccessServiceResult);
            var result = _menuService.UpdateMenu(MenuServiceTestData.NewMenu);
            _menuRepository.Verify(x => x.Update(It.IsAny<Menu>()), Times.Once, "Should update menu using repository");
        }

        [TestMethod]
        public void UpdateMenu_ShouldCommitChanges()
        {
            _menuServiceMockHelper.Setup(x => x.GetUpdateResult(It.IsAny<bool>())).Returns(MenuServiceTestData.SuccessServiceResult);
            var result = _menuService.UpdateMenu(MenuServiceTestData.NewMenu);
            _uow.Verify(x => x.Commit(), Times.Once, "Should commit changes after adding the menu.");
        }

        [TestMethod]
        public void UpdateMenu_ItemUpdated_ShouldGetServiceResult()
        {
            var result = _menuService.UpdateMenu(MenuServiceTestData.NewMenu);

            _menuServiceMockHelper.Verify(x => x.GetUpdateResult(It.IsAny<bool>()), Times.Once, "Should get result from service result");
        }

        #endregion

        #region RemoveMenu
        [TestMethod]
       
        [DataRow(1, false)]
        [DataRow(1, true)]
        public void RemoveMenu_WithOrWithoutCascade_ShouldAddNewMenuUsingRepository(int id, bool cascadeRemove = false)
        {
            var result = _menuService.RemoveMenu(id, cascadeRemove);
            _menuRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Once, "Should delete using repository");
        }

        [TestMethod]
        [DataRow(1, false)]
        [DataRow(1, true)]
        public void RemoveMenu_WithOrWithoutCascade_ShouldCommitChanges(int id, bool cascadeRemove = false)
        {
            var result = _menuService.RemoveMenu(id, cascadeRemove);
            _uow.Verify(x => x.Commit(), Times.Once, "Should commit changes after adding the menu.");
        }

        [TestMethod]
        [DataRow(1, false)]
        [DataRow(1, true)]
        public void RemoveMenu_ItemDeleted_ShouldGetServiceResult(int id, bool cascadeRemove= false)
        {
            var result = _menuService.RemoveMenu(id, cascadeRemove);
            _menuServiceMockHelper.Verify(x => x.GetRemoveResult(It.IsAny<bool>()), Times.Once, "Should get result from service result");
        }
        [TestMethod]
        public void RemoveMenu_CascadeDelete_ShouldRemoveAllMenuItems()
        {
            _menuItemRepository.Setup(x => x.GetAll(It.IsAny<Expression<Func<MenuItem, bool>>>(),It.IsAny<Expression<Func<MenuItem, object>>[]>())).Returns(MenuServiceTestData.MenuItems.AsQueryable());
            var result = _menuService.RemoveMenu(1, true);
            _menuItemRepository.Verify(x => x.DeleteRange(It.IsAny<ICollection<MenuItem>>()), Times.Once, "Should delete all menu items");
        }


        #endregion

        #region GetAllMenusBySelectListItem
        [TestMethod]
        public void GetAllMenusBySelectListItem()
        {
            var result = _menuService.GetAllMenusBySelectListItem(2);
            _menuRepository.Verify(x => x.GetAll(), Times.Once, "Should Get all Menus");
        }

        [TestMethod]
        public void GetAllMenusBySelectListItem_SelectedShouldBeTrue()
        {
            _menuRepository.Setup(x => x.GetAll()).Returns(MenuServiceTestData.Menus.AsQueryable());
            var result = _menuService.GetAllMenusBySelectListItem(1);
            result.TrueForAll(x => x.Selected);
        }

        [TestMethod]
        public void GetAllMenusBySelectListItem_SelectedShouldBeFalse()
        {
            _menuRepository.Setup(x => x.GetAll()).Returns(MenuServiceTestData.Menus.AsQueryable());
            var result = _menuService.GetAllMenusBySelectListItem(2);
            result.TrueForAll(x => !x.Selected);
        }
        #endregion

        [TestInitialize]
        public void GetServiceInstance()
        {
            _menuService = new MenuServiceWrapper(_uow.Object, _configurationProvider.Object, _menuServiceMockHelper.Object);
            _uow.Setup(x => x.MenuRepository).Returns(_menuRepository.Object);
            _uow.Setup(x => x.MenuItemRepository).Returns(_menuItemRepository.Object);
        }
    }
}
