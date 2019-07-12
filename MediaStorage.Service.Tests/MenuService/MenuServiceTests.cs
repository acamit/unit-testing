using MediaStorage.Config;
using MediaStorage.Data;
using MediaStorage.Data.Entities;
using MediaStorage.Service.Tests.MenuServiceTests;
using MediaStorage.Service.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

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
        [TestInitialize]
        public void GetServiceInstance()
        {
            _menuService = new MenuServiceWrapper(_uow.Object, _configurationProvider.Object, _menuServiceMockHelper.Object);
            _uow.Setup(x => x.MenuRepository).Returns(_menuRepository.Object);
            _uow.Setup(x => x.MenuItemRepository).Returns(_menuItemRepository.Object);
        }
    }
}
